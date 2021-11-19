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

        private void ParseFilesInDir(VeederRootOptions opts)
        {
            List<string> filesToConvert = GetFilesInDirectory(opts.FilePath);

            foreach (var file in filesToConvert)
            {
                _parser.FilePath = file;
                _parser.Parse();

                var newDirectory = CreateNewDirectoryName(opts, _parser);

                // TODO: This is bad.
                if (_parser.TankTables is not null)
                {
                    if (opts.CreateFuelPosFile)
                        POSFileCreator.CreateTmsAofFile(_parser.TankTables, newDirectory);

                    if (opts.CreateCsv)
                        POSFileCreator.CreateFuelPosSetupCsv(_parser as VdrRootFileParser, newDirectory);
                }
            }
        }

        private static string CreateNewDirectoryName(VeederRootOptions opts, IVdrRootFileParser parser)
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

        private static List<string> GetFilesInDirectory(string directoryPath) 
        {
            var files = Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(f => f.EndsWith(".cal") || f.EndsWith(".txt") || f.EndsWith(".cap"))
                .ToList();

            return files;
        }
            
    }
}
