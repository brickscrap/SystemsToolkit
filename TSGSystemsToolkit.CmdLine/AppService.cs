using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System;
using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Rendering;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.Commands;
using TSGSystemsToolkit.CmdLine.GraphQL;

namespace TSGSystemsToolkit.CmdLine
{
    internal class AppService : IAppService
    {
        private readonly ILogger<AppService> _logger;
        private readonly IRootCommands _rootCommands;
        private readonly IConfiguration _config;
        private readonly SysTkApiClient _client;

        public AppService(ILogger<AppService> logger, IRootCommands rootCommands, IConfiguration config, SysTkApiClient client)
        {
            _logger = logger;
            _rootCommands = rootCommands;
            _config = config;
            _client = client;
        }

        public async Task<int> Run(string[] args)
        {
            //Console.WriteLine("Connecting to client.");
            
            //var result = await _client.GetToken.ExecuteAsync("pow@use.r", "3A2o7z%$iE$6FxoRj6NBL8ZwVJ");
            //if (result.Errors.Count == 0)
            //    Extensions.UpdateAccessToken(result.Data.Login.AccessToken);
            //else
            //    Console.WriteLine("Error collecting token");
            
            //return 0;

            var cmd = _rootCommands.Create();
            cmd.Name = "SysTk";
            cmd.Description = "A series of command-line tools to help ease the burden of your left-click button.";

            AnsiConsole.Write(new FigletText("TSG Systems Toolkit").Color(Color.OrangeRed1));

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

            return await cmd.InvokeAsync(args);
        }

        private void CheckForUpdates()
        {
            if (Extensions.IsUpdateAvailable(_config.GetValue<string>("MasterLocation")))
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"A new version of SystemsToolkit is available!");
                Console.WriteLine($"Use command: {ForegroundColorSpan.Yellow()}systk --update{ForegroundColorSpan.Reset()} to update automatically.");

                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
    }
}
