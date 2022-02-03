using Microsoft.Extensions.Logging;
using Pse.TerminalsToEmis;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public class TerminalsHandler : AbstractHandler<TerminalsOptions>
    {
        private readonly ILogger<TerminalsHandler> _logger;

        public TerminalsHandler(ILogger<TerminalsHandler> logger)
        {
            _logger = logger;
        }

        public override async Task<int> RunHandlerAndReturnExitCode(TerminalsOptions options, CancellationToken ct = default(CancellationToken))
        {
            if (options.CreateEmisFile)
            {
                string outputPath;

                if (string.IsNullOrWhiteSpace(options.OutputPath))
                {
                    outputPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    outputPath = $"{outputPath}\\FuelPos";
                    if (!Directory.Exists(outputPath))
                        throw new DirectoryNotFoundException($"Directory {outputPath} could not be found. Is Remote eMIS installed?");

                    _logger.LogDebug("Output path automatically set to: {Output}", outputPath);
                }
                else
                {
                    outputPath = options.OutputPath;
                    _logger.LogDebug("Output path manually defined at: {Output}", outputPath);
                }

                TerminalsToEmis.Run($"{options.FilePath}\\Terminals_044.csv", outputPath);
            }

            return 0;
        }
    }
}
