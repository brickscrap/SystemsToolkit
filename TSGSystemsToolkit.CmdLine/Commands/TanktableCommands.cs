using FuelPOS.TankTableTools;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Commands
{
    public static class TanktableCommands
    {
        public static void ParseSingleGaugeFile(TankTableOptions opts)
        {
            GaugeFileParser parser = new();
            parser = ParseFile(parser, opts);
            var outputDir = opts.OutputPath;

            if (string.IsNullOrWhiteSpace(opts.OutputPath))
            {
                outputDir = Path.GetDirectoryName(opts.GaugeFilePath);
            }
            
            if (opts.CreateFuelPosFile)
            {
                
                POSFileCreator.CreateTmsAofFile(parser.TankTables, outputDir);
            }

            if (opts.CreateTankSetupFile)
            {
                POSFileCreator.CreateFuelPosSetupCsv(parser, outputDir);
            }
        }

        public static void ParseGaugeFilesInDir(TankTableOptions opts)
        {
            var parsers = CreateParsers(opts);

            foreach (var p in parsers)
            {
                var newDirectory = CreateNewDirectoryName(opts, p);

                if (p.TankTables is not null)
                {
                    if (opts.CreateFuelPosFile)
                    {
                        POSFileCreator.CreateTmsAofFile(p.TankTables, newDirectory);
                    }

                    if (opts.CreateTankSetupFile)
                    {
                        POSFileCreator.CreateFuelPosSetupCsv(p, newDirectory);
                    }
                }
            }
        }

        public static void ParseBasicFileInDir(TankTableOptions opts)
        {
            BasicFileParser parser = new(opts.DirectoryPath);

            parser.LoadFilesAndParse();

            if (parser.TankTables is not null)
            {
                if (opts.CreateFuelPosFile)
                {
                    POSFileCreator.CreateTmsAofFile(parser.TankTables, opts.DirectoryPath);
                }
            }
        }

        private static List<GaugeFileParser> CreateParsers(TankTableOptions opts)
        {
            GaugeFileParser parser;

            List<string> filesToConvert = GetFilesInDirectory(opts.DirectoryPath);
            List<GaugeFileParser> parsers = new();

            foreach (var file in filesToConvert)
            {
                parser = new(file);
                parser.Parse(opts.FullFile);
                parsers.Add(parser);
            }

            return parsers;
        }

        private static string CreateNewDirectoryName(TankTableOptions opts, GaugeFileParser parser)
        {
            string newDirectory;
            if (string.IsNullOrWhiteSpace(parser.SiteName))
            {
                newDirectory = $"{opts.DirectoryPath}\\{Path.GetFileNameWithoutExtension(parser.FilePath)}";
            }
            else
            {
                newDirectory = $"{opts.DirectoryPath}\\{parser.SiteName}";
            }

            return newDirectory;
        }

        private static GaugeFileParser ParseFile(GaugeFileParser parser, TankTableOptions opts)
        {
            parser.FilePath = opts.GaugeFilePath;

            parser.Parse(opts.FullFile);

            return parser;
        }

        private static List<string> GetFilesInDirectory(string directoryPath)
        {
            return Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(f => f.EndsWith("*.cal") || f.EndsWith("*.txt") || f.EndsWith("*.cap"))
                .ToList();
        }
    }
}
