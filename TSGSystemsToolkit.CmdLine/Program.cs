using CommandLine;
using FuelPOS.DebugTools;
using FuelPOS.StatDevParser;
using FuelPOS.StatDevParser.Models;
using FuelPOS.TankTableTools;
using Newtonsoft.Json;
using POSFileParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TSGSystemsToolkit.CmdLine.Commands;
using TSGSystemsToolkit.CmdLine.Options;
using TSGSystemsToolkit.CmdLine.TestFuncs;

namespace TSGSystemsToolkit.CmdLine
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<TankTableOptions, object>(args)
                .MapResult(
                (TankTableOptions opts) => RunTankTablesAndReturnExitCode(opts),
                errs => HandleParserError(errs));
        }

        public static int RunTankTablesAndReturnExitCode(TankTableOptions opts)
        {
            var exitCode = 0;

            if (string.IsNullOrWhiteSpace(opts.DirectoryPath))
            {
                TanktableCommands.ParseSingleFile(opts);
            }
            else
            {
                TanktableCommands.ParseFilesInDirectory(opts);
            }

            return exitCode;
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

        
    }
}
