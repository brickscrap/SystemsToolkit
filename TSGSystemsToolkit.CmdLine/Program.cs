using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using TSGSystemsToolkit.CmdLine.Commands;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine
{
    class Program
    {
        static void Main(string[] args)
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

            if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError))
            {
                result = -1;
            }

            if (result < 0 && result is not - 1)
            {
                Console.WriteLine($"Error - the program exitted with exit code {result}.");
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
    }
}
