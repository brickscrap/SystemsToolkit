using CommandLine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using TSGSystemsToolkit.CmdLine.Commands;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine
{
    class Program
    {
        static void Main(string[] args)
        {
            CheckForUpdates();

            ParseArgs(args);
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

        private static void ParseArgs(string[] args)
        {
            ParserResult<object> parsed = Parser.Default
                .ParseSetArguments<FuelPosVerbSet>(args,
                                                   OnVerbSetParsed,
                                                   typeof(TankTableOptions),
                                                   typeof(PseOptions),
                                                   typeof(SurveyOptions));

            parsed.MapResult(
                (TankTableOptions opts) => RunTankTablesAndReturnExitCode(opts),
                (PseOptions opts) => RunPseAndReturnExitCode(opts),
                (SurveyOptions opts) => RunSurveyAndReturnExitCode(opts),
                (CreateMutationOptions opts) => RunCreateMutationAndReturnExitCode(opts),
                errs => HandleParserError(errs));
        }

        public static int RunTankTablesAndReturnExitCode(TankTableOptions opts)
        {
            var exitCode = 0;

            if (string.IsNullOrWhiteSpace(opts.DirectoryPath))
            {
                TanktableCommands.ParseSingleGaugeFile(opts);
            }
            else
            {
                if (opts.FromTextFiles)
                {
                    TanktableCommands.ParseBasicFileInDir(opts);
                }
                else
                {
                    TanktableCommands.ParseGaugeFilesInDir(opts);
                }
            }

            return exitCode;
        }

        public static int RunPseAndReturnExitCode(PseOptions opts)
        {
            var exitCode = 0;

            if (!string.IsNullOrWhiteSpace(opts.TerminalsFilePath))
            {
                PseCommands.RunTerminalsCommands(opts);
            }

            return exitCode;
        }

        public static int RunSurveyAndReturnExitCode(SurveyOptions opts)
        {
            var exitCode = 0;

            SurveyCommands.RunSurveyCommands(opts);

            return exitCode;
        }

        public static int RunCreateMutationAndReturnExitCode(CreateMutationOptions opts)
        {
            FuelPosCommands.RunCreateMutationCommands(opts);

            return 0;
        }

        public static int HandleParserError(IEnumerable<Error> errs)
        {
            var result = -2;

            if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError || x is HelpVerbRequestedError))
            {
                result = -1;
            }

            if (result < -1)
            {
                Console.WriteLine($"Error - the program exited with code {result}.");
                foreach (var e in errs)
                {
                    Console.WriteLine($"Error - {e}.");
                }
            }

            return result;
        }

        private static ParserResult<object> OnVerbSetParsed(Parser parser, Parsed<object> parsed, IEnumerable<string> args, bool containedHelpOrVersion)
        {
            return parsed.MapResult(
                    (FuelPosVerbSet _) => parser.ParseArguments<object, CreateMutationOptions>(args),
                    (_) => parsed);
        }        

        private static bool IsUpdateAvailable(string currentVersion, string masterVersion)
        {
            var current = currentVersion.Split('.')
                .Select(x => int.Parse(x))
                .ToList();
            var master = masterVersion
                .Split('.')
                .Select(x => int.Parse(x))
                .ToList();

            var output = current.Except(master);

            return output.Any();
        }

        
    }
}
