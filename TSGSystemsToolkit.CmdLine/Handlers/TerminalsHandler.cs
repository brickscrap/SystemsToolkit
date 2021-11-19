using Microsoft.Extensions.Logging;
using Pse.TerminalsToEmis;
using System;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public class TerminalsHandler : ITerminalsHandler
    {
        private readonly ILogger<TerminalsHandler> _logger;

        // TODO: Add logging & exception handling
        public TerminalsHandler(ILogger<TerminalsHandler> logger)
        {
            _logger = logger;
        }

        public int RunHandlerAndReturnExitCode(TerminalsOptions options)
        {
            if (options.CreateEmisFile)
            {
                string outputPath;

                if (string.IsNullOrWhiteSpace(options.OutputPath))
                {
                    outputPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    outputPath = $"{outputPath}\\FuelPos";
                }
                else
                {
                    outputPath = options.OutputPath;
                }

                TerminalsToEmis.Run($"{options.FilePath}\\Terminals_044.csv", outputPath);
            }

            return 1;
        }
    }
}
