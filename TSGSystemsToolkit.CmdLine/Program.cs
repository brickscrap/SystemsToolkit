using FuelPOS.StatDevParser;
using FuelPOS.TankTableTools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SysTk.DataManager.DataAccess;
using SysTk.DataManager.Ftp;
using TSGSystemsToolkit.CmdLine.Commands;
using TSGSystemsToolkit.CmdLine.GraphQL;
using TSGSystemsToolkit.CmdLine.Services;

namespace TSGSystemsToolkit.CmdLine
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            var absolutePath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);
            var config = builder
                .SetBasePath(absolutePath)
                .AddJsonFile(@$"{absolutePath}\appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code, outputTemplate: "[{Level:u3}] {Message:lj} {NewLine}")
                .CreateLogger();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton(context.Configuration);
                    services.AddTransient<IAppService, AppService>();
                    services.AddTransient<IRootCommands, RootCommands>();

                    // Data Access
                    services.AddTransient<IFtpService, FtpService>();
                    services.AddTransient<ISqliteDataAccess, SqliteDataAccess>();
                    services.AddTransient<ICardIdentificationData, CardIdentificationData>();
                    services.AddTransient<IAuthService, AuthService>();

                    services.AddHttpClient<SysTkApiClient>(c =>
                    {

                    }).ConfigurePrimaryHttpMessageHandler(() =>
                    {
                        return new HttpClientHandler
                        {
                            ClientCertificateOptions = ClientCertificateOption.Manual,
                            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, certChain, policyErrors) => true
                        };
                    });
                    services.AddSysTkApiClient()
                        .ConfigureHttpClient(client =>
                        {
                            client.BaseAddress = new Uri(config["GraphQLAddress"]);
                            client.DefaultRequestHeaders.Authorization =
                                new AuthenticationHeaderValue("bearer", config["AccessToken"]);
                        });

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
