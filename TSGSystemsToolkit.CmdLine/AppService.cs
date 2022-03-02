using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using StrawberryShake;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Parsing;
using System.Linq;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.Commands;
using TSGSystemsToolkit.CmdLine.Constants;
using TSGSystemsToolkit.CmdLine.GraphQL;
using TSGSystemsToolkit.CmdLine.Services;

namespace TSGSystemsToolkit.CmdLine
{
    internal class AppService : IAppService
    {
        private readonly ILogger<AppService> _logger;
        private readonly IRootCommands _rootCommands;
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;

        public AppService(ILogger<AppService> logger, IRootCommands rootCommands, IConfiguration config, IAuthService authService)
        {
            _logger = logger;
            _rootCommands = rootCommands;
            _config = config;
            _authService = authService;
        }

        public async Task<int> Run(string[] args)
        {
            var cmd = _rootCommands.Create();
            cmd.Name = "SysTk";
            cmd.Description = "A series of command-line tools to help ease the burden of your left-click button.";

            AnsiConsole.Write(new FigletText("TSG SysTk").Color(Color.OrangeRed1));

            try
            {
                CheckForUpdates();
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error checking for updates: {Exception}", ex.Message);
                _logger.LogDebug("Inner exception: {Inner}", ex.InnerException);
                _logger.LogDebug("Trace: {Trace}", ex.StackTrace);
            }

            if (args.Length == 0)
            {
                var helpBuilder = new HelpBuilder(LocalizationResources.Instance);
                helpBuilder.Write(cmd, Console.Out);
                return 0;
            }

            if(string.IsNullOrWhiteSpace(_config["EmailAddress"]))
            {
                var email = AnsiConsole.Prompt(new TextPrompt<string>("Enter the email addess associated with your API account:")
                    .PromptStyle("green"));

                _config["EmailAddress"] = email;
                Extensions.UpdateEmailAddress(email);
            }

            var commandLineBuilder = new CommandLineBuilder(cmd);
            commandLineBuilder.AddMiddleware(async (context, next) =>
            {
                try
                {
                    await next(context);
                }
                catch (GraphQLClientException ex)
                {
                    if(ex.Errors.Any(x => x.Code == ApiErrorCodes.NotAuthenticated))
                    {
                        var authResult = await _authService.AuthSimple();

                        if (authResult)
                            await next(context);
                    }
                }
            });

            commandLineBuilder.UseTypoCorrections();
            commandLineBuilder.UseHelp();

            //commandLineBuilder.UseExceptionHandler((x, y) =>
            //{
                
            //}, 1);

            var parser = commandLineBuilder.Build();

            return await parser.InvokeAsync(args);
        }

        private void CheckForUpdates()
        {
            if (Extensions.IsUpdateAvailable(_config.GetValue<string>("MasterLocation")))
            {
                AnsiConsole.MarkupLine($"A new version of SystemsToolkit is available!");
                AnsiConsole.MarkupLine($"Use command: [yellow]systk --update[/] to update automatically.");
            }
        }
    }
}
