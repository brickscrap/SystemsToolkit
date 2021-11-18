using FuelPOS.TankTableTools;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public class VeederRootHandler : IVeederRootHandler
    {
        private IVdrRootFileParser _parser;
        private readonly ILogger<VeederRootHandler> _logger;

        public VeederRootHandler(IVdrRootFileParser parser, ILogger<VeederRootHandler> logger)
        {
            _parser = parser;
            _logger = logger;
        }

        public int RunHandlerAndReturnExitCode(VeederRootOptions options)
        {
            // TODO: Validate file path, handle exceptions
            FileAttributes attr = File.GetAttributes(options.FilePath);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                ParseFilesInDir(options);

                return 1;
            }
            else
            {
                ParseSingleFile(options);

                return 1;
            }
        }

        private void ParseSingleFile(VeederRootOptions opts)
        {
            _parser = ParseFile(_parser, opts);
            var outputDir = opts.OutputPath;

            if (string.IsNullOrWhiteSpace(opts.OutputPath))
            {
                outputDir = Path.GetDirectoryName(opts.FilePath);
            }

            if (opts.CreateFuelPosFile)
                POSFileCreator.CreateTmsAofFile(_parser.TankTables, outputDir);

            if (opts.CreateCsv)
                POSFileCreator.CreateFuelPosSetupCsv(_parser as VdrRootFileParser, outputDir);
        }

        private static void ParseFilesInDir(VeederRootOptions opts)
        {
            var parsers = CreateParsers(opts);

            foreach (var p in parsers)
            {
                var newDirectory = CreateNewDirectoryName(opts, p);

                if (p.TankTables is not null)
                {
                    if (opts.CreateFuelPosFile)
                        POSFileCreator.CreateTmsAofFile(p.TankTables, newDirectory);

                    if (opts.CreateCsv)
                        POSFileCreator.CreateFuelPosSetupCsv(p, newDirectory);
                }
            }
        }

        private static List<VdrRootFileParser> CreateParsers(VeederRootOptions opts)
        {
            VdrRootFileParser parser;

            List<string> filesToConvert = GetFilesInDirectory(opts.FilePath);
            List<VdrRootFileParser> parsers = new();

            foreach (var file in filesToConvert)
            {
                parser = new(file);
                parser.Parse();
                parsers.Add(parser);
            }

            return parsers;
        }

        private static string CreateNewDirectoryName(VeederRootOptions opts, VdrRootFileParser parser)
        {
            string newDirectory;
            if (string.IsNullOrWhiteSpace(parser.SiteName))
                newDirectory = $"{opts.FilePath}\\{Path.GetFileNameWithoutExtension(parser.FilePath)}";
            else
                newDirectory = $"{opts.FilePath}\\{parser.SiteName}";

            return newDirectory;
        }

        private static IVdrRootFileParser ParseFile(IVdrRootFileParser parser, VeederRootOptions opts)
        {
            parser.FilePath = opts.FilePath;

            parser.Parse();

            return parser;
        }

        private static List<string> GetFilesInDirectory(string directoryPath) =>
            Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(f => f.EndsWith("*.cal") || f.EndsWith("*.txt") || f.EndsWith("*.cap"))
                .ToList();
    }
}
