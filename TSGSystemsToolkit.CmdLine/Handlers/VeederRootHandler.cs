using FuelPOS.TankTableTools;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public class VeederRootHandler : AbstractHandler<VeederRootOptions>
    {
        private IVdrRootFileParser _parser;
        private readonly ILogger<VeederRootHandler> _logger;

        public VeederRootHandler() : base()
        {

        }

        public VeederRootHandler(IVdrRootFileParser parser, ILogger<VeederRootHandler> logger)
        {
            _parser = parser;
            _logger = logger;
        }

        public override async Task<int> RunHandlerAndReturnExitCode(VeederRootOptions options)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(options.FilePath);

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    _logger.LogInformation("Parsing files in directory: {Directory}.", options.FilePath);
                    ParseFilesInDir(options);

                    return 0;
                }
                else
                {
                    ParseSingleFile(options);

                    return 0;
                }
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError("Error: {Message}", ex.Message);
                _logger.LogDebug("Inner Exception: {Inner}", ex.InnerException);
                _logger.LogDebug("Trace: {Trace}", ex.StackTrace);

                return -1;
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

            HandleBoolOptions(opts, outputDir);
        }

        private void HandleBoolOptions(VeederRootOptions opts, string outputDir)
        {
            if (opts.CreateFuelPosFile)
            {
                _logger.LogInformation("Creating TMS_AOF.INP at {OutputDir}", outputDir);
                POSFileCreator.CreateTmsAofFile(_parser.TankTables, outputDir);
            }

            if (opts.CreateCsv)
            {
                _logger.LogInformation("Creating tank setup CSV at {OutputDir}", outputDir);
                try
                {
                    POSFileCreator.CreateFuelPosSetupCsv(_parser as VdrRootFileParser, outputDir);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error creating file at {outputDir}\n" +
                        "{Message}", outputDir, ex.Message);
                }
            }
        }

        private void ParseFilesInDir(VeederRootOptions opts)
        {
            List<string> filesToConvert = GetFilesInDirectory(opts.FilePath);

            foreach (var file in filesToConvert)
            {
                _parser.FilePath = file;
                _parser.Parse();

                var outputDir = CreateNewDirectoryName(opts, _parser);

                HandleBoolOptions(opts, outputDir);
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
