using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.Commands;
using System.CommandLine.Builder;
using Spectre.Console;
using System.CommandLine.Help;

namespace TSGSystemsToolkit.CmdLine
{
    internal class AppService : IAppService
    {
        private readonly ILogger<AppService> _log;
        private readonly IRootCommands _rootCommands;

        public AppService(ILogger<AppService> log, IRootCommands rootCommands)
        {
            _log = log;
            _rootCommands = rootCommands;
        }

        public async Task<int> Run(string[] args)
        {
            // TODO: Better update method
            // CheckForUpdates();

            var cmd = _rootCommands.Create();
            cmd.Name = "SysTk";
            cmd.Description = "A series of command-line tools to help ease the burden of your left-click button.";

            AnsiConsole.Write(new FigletText("TSG Systems Toolkit").Color(Color.OrangeRed1));

            if (args.Length == 0)
            {
                var helpBuilder = new HelpBuilder(LocalizationResources.Instance);
                helpBuilder.Write(cmd, Console.Out);
                return 1;
            }

            return await cmd.InvokeAsync(args);
        }

        private static void CheckForUpdates()
        {
            var updater = new { MasterLocation = "" };
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            using (StreamReader r = File.OpenText(Path.Combine(baseDir, "updater.json")))
            {
                string json = r.ReadToEnd();
                updater = JsonConvert.DeserializeAnonymousType(json, updater);
            };

            var currentVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            var masterVersion = FileVersionInfo.GetVersionInfo(Path.Combine(updater.MasterLocation, "SysTk.exe")).FileVersion;

            if (IsUpdateAvailable(currentVersion, masterVersion))
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"A new version of SystemsToolkit is available!");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine($"Current Version: {currentVersion}");
                Console.WriteLine($"Version Available: {masterVersion}");
                Console.WriteLine($"To update, copy all files from \"{updater.MasterLocation}\" to \"{baseDir}\"");

                Console.WriteLine();
            }
        }
        private static bool IsUpdateAvailable(string currentVersion, string masterVersion)
        {
            for (int i = 0; i < currentVersion.Split('.').Length; i++)
            {
                var master = masterVersion.Split('.');
                var current = currentVersion.Split('.');

                if (int.Parse(master[i]) > int.Parse(current[i]) || current.Length > master.Length)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
