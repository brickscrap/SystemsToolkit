using FuelPOS.StatDevParser;
using FuelPOS.TankTableTools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
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
        static async Task<int> Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            var absolutePath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);
            var provider = new PhysicalFileProvider(absolutePath);
            var config = builder
                .SetBasePath(absolutePath)
                .AddJsonFile(@$"{absolutePath}\appsettings.json", false, true)
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
                    app.AddConfiguration(config);
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<AppService>(host.Services);

            return await svc.Run(args);
        }
    }
}
