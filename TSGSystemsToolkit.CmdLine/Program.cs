using FuelPOS.StatDevParser;
using FuelPOS.TankTableTools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.IO;
using System.Threading.Tasks;
using SysTk.DataManager.DataAccess;
using SysTk.Utils;
using TSGSystemsToolkit.CmdLine.Commands;
using TSGSystemsToolkit.CmdLine.Handlers;

namespace TSGSystemsToolkit.CmdLine
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            var config = builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton(context.Configuration);
                    services.AddTransient<IAppService, AppService>();
                    services.AddTransient<IRootCommands, RootCommands>();

                    // Data Access
                    services.AddTransient<IFtpHandler, FtpHandler>();
                    services.AddTransient<ISqliteDataAccess, SqliteDataAccess>();
                    services.AddTransient<ICardIdentificationData, CardIdentificationData>();

                    // Handlers
                    services.AddTransient<IVeederRootHandler, VeederRootHandler>();
                    services.AddTransient<IProgaugeHandler, ProgaugeHandler>();
                    services.AddTransient<ITerminalsHandler, TerminalsHandler>();
                    services.AddTransient<IMutationHandler, MutationHandler>();
                    services.AddTransient<ISurveyHandler, SurveyHandler>();
                    services.AddTransient<IUpdateHandler, UpdateHandler>();

                    // Business Services
                    services.AddTransient<IVdrRootFileParser, VdrRootFileParser>();
                    services.AddTransient<IProgaugeFileParser, ProgaugeFileParser>();
                    services.AddTransient<IStatDevParser, StatDevParser>();
                })
                .ConfigureAppConfiguration(app =>
                {
                    //var relativePath = @".";
                    //var absolutePath = Path.GetFullPath(relativePath);
                    //var provider = new PhysicalFileProvider(absolutePath);
                    //app.AddJsonFile(provider, "appsettings.json", optional: false, reloadOnChange: true);
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<AppService>(host.Services);

            await svc.Run(args);
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
