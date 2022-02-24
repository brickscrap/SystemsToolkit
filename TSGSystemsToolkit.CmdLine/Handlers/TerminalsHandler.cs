using Microsoft.Extensions.Logging;
using Pse.TerminalsToEmis;
using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public class TerminalsHandler : ICommandHandler
    {
        private ILogger<TerminalsHandler> _logger;
        private readonly TerminalsOptions _options;
        private readonly CancellationToken _ct;

        public TerminalsHandler(TerminalsOptions options, CancellationToken ct = default)
        {
            _options = options;
            _ct = ct;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            GetDependencies(context);

            if (_options.CreateEmisFile)
            {
                string outputPath;

                if (string.IsNullOrWhiteSpace(_options.OutputPath))
                {
                    outputPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    outputPath = $"{outputPath}\\FuelPos";
                    if (!Directory.Exists(outputPath))
                    {
                        throw new DirectoryNotFoundException($"Directory {outputPath} could not be found. Is Remote eMIS installed?");
                    }

                    _logger.LogDebug("Output path automatically set to: {Output}", outputPath);
                }
                else
                {
                    outputPath = _options.OutputPath;
                    _logger.LogDebug("Output path manually defined at: {Output}", outputPath);
                }

                TerminalsToEmis.Run($"{_options.FilePath}\\Terminals_044.csv", outputPath);
            }

            return 0;
        }

        private void GetDependencies(InvocationContext context)
        {
            _logger = context.BindingContext.GetService(typeof(ILogger<TerminalsHandler>)) as ILogger<TerminalsHandler>;
        }
    }
}
