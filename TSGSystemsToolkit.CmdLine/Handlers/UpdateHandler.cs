using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public class UpdateHandler : ICommandHandler
    {
        private IConfiguration _config;
        private ILogger<UpdateHandler> _logger;
        private readonly UpdateOptions _options;
        private readonly CancellationToken _ct;

        public UpdateHandler(UpdateOptions options, IConfiguration config, CancellationToken ct = default)
        {
            _options = options;
            _config = config;
            _ct = ct;
        }

        private void GetDependencies(InvocationContext context)
        {
            _logger = context.BindingContext.GetService(typeof(ILogger<UpdateHandler>)) as ILogger<UpdateHandler>;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            GetDependencies(context);

            var assemblyVersion = Assembly.GetEntryAssembly().GetName().Version;
            Version currentVersion = new(assemblyVersion.Major.ToString(), assemblyVersion.Minor.ToString(), assemblyVersion.Build.ToString());
            _logger.LogInformation("Current version: {Current}", currentVersion);

            Version available = Extensions.GetAvailableVersion(_config.GetValue<string>("MasterLocation"));
            _logger.LogInformation("Available version: {Available}", available);

            if (available > currentVersion)
            {
                _logger.LogInformation("Update available! Updating...");

                Process.Start(available.InstallerPath);
            }

            return 0;
        }
    }
}
