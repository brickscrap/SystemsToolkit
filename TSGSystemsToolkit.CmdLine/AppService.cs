using Microsoft.Extensions.Configuration;
using StrawberryShake;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Parsing;
using TSGSystemsToolkit.CmdLine.Commands;
using TSGSystemsToolkit.CmdLine.Constants;
using TSGSystemsToolkit.CmdLine.Services;

namespace TSGSystemsToolkit.CmdLine;

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

        if (string.IsNullOrWhiteSpace(_config["EmailAddress"]))
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
                if (ex.Errors.Any(x => x.Code == ApiErrorCodes.NotAuthenticated))
                {
                    var authResult = await _authService.AuthSimple();

                    if (authResult)
                        await next(context);
                }
            }
        });

        commandLineBuilder.AddMiddleware(async (context, next) =>
        {
            if (context.BindingContext.ParseResult.Errors.Count > 0)
            {
                Console.WriteLine();
                foreach (var item in context.BindingContext.ParseResult.Errors)
                {
                    AnsiConsole.MarkupLine($"[red]{item.Message}[/]");
                }
                Console.WriteLine();
                var helpBuilder = new HelpBuilder(LocalizationResources.Instance);
                helpBuilder.Write(context.ParseResult.CommandResult.Command, Console.Out);

                return;
            }

            await next(context);
        });

        commandLineBuilder.UseTypoCorrections();
        commandLineBuilder.UseHelp();

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
