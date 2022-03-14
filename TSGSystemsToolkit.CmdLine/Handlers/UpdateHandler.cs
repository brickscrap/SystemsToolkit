using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spectre.Console;
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

        public UpdateHandler(UpdateOptions options, CancellationToken ct = default)
        {
            _options = options;
            _ct = ct;
        }

        private void GetDependencies(InvocationContext context)
        {
            _config = context.BindingContext.GetService(typeof(IConfiguration)) as IConfiguration;
            _logger = context.BindingContext.GetService(typeof(ILogger<UpdateHandler>)) as ILogger<UpdateHandler>;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            GetDependencies(context);

            var fileVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version.Split('.');
            Version currentVersion = new(fileVersion[0], fileVersion[1], fileVersion[2]);
            AnsiConsole.MarkupLine($"Current version: [orange]{currentVersion}[/]");

            Version available = Extensions.GetAvailableVersion(_config.GetValue<string>("MasterLocation"));
            AnsiConsole.MarkupLine($"Available version: [green]{available}[/]");

            if (available > currentVersion)
            {
                AnsiConsole.MarkupLine("Update available! [green]Updating...[/]");

                Process.Start(available.InstallerPath, "/S");
            }

            return 0;
        }
    }
}
