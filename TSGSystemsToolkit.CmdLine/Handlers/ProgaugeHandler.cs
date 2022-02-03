using FuelPOS.TankTableTools;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public class ProgaugeHandler : AbstractHandler<ProgaugeOptions>
    {
        // TODO: Add logging/output
        private readonly ILogger<ProgaugeHandler> _logger;
        private readonly IProgaugeFileParser _parser;

        public ProgaugeHandler(ILogger<ProgaugeHandler> logger, IProgaugeFileParser parser)
        {
            _logger = logger;
            _parser = parser;
        }

        public override async Task<int> RunHandlerAndReturnExitCode(ProgaugeOptions options, CancellationToken ct = default(CancellationToken))
        {
            // TODO: Validate file path, handle exceptions
            ParseBasicFileInDir(options);

            return 1;
        }

        private void ParseBasicFileInDir(ProgaugeOptions opts)
        {
            _parser.FolderPath = opts.FilePath;

            _parser.LoadFilesAndParse();

            if (_parser.TankTables is not null)
            {
                if (opts.CreateFuelPosFile)
                {
                    POSFileCreator.CreateTmsAofFile(_parser.TankTables, opts.FilePath);
                }
            }
        }

        private static List<string> GetFilesInDirectory(string directoryPath)
        {
            return Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(f => f.EndsWith("*.cal") || f.EndsWith("*.txt") || f.EndsWith("*.cap"))
                .ToList();
        }
    }
}
