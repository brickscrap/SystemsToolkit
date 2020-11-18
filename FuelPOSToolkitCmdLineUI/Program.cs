using System;
using System.Collections.Generic;
using System.Xml.Linq;
using ToolkitLibrary;
using ToolkitLibrary.Models;
using POSFileParser;
using TankTableToolkit;
using POSFileParser.Attributes;
using POSFileParser.Models;
using System.Globalization;
using System.Threading;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;
using FuelPOSDebugTools;
using FuelPOSFTPLib;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;

namespace FuelPOSToolkitCmdLineUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"C:\Users\omgit\source\repos\FuelPOSToolkit\FuelPOSToolkitCmdLineUI\Log.log")
                .CreateLogger();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var ftpService = serviceProvider.GetService<FtpConnection>();
            ftpService.HostName = "brickscrap.cloud.seedboxes.cc";
            ftpService.UserName = "brickscrap";
            ftpService.Password = "Hosted19";
            ftpService.PortNumber = 9979;

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            FTPTests(ftpService);

            watch.Stop();
            Console.WriteLine($"Execution time: {watch.ElapsedMilliseconds}");
            Console.ReadLine();
        }

        public static void ParserTests()
        {
            var output = Parser.LoadTRXFile("C:\\Users\\omgit\\source\\repos\\FuelPOSToolkit\\POSFileParser\\Tests\\11841179.TRX");
            // Parser.LoadFile("C:\\Users\\omgit\\source\\repos\\FuelPOSToolkit\\POSFileParser\\Tests\\11841179.TRX");
            // Parser.LoadFile("C:\\Users\\omgit\\source\\repos\\FuelPOSToolkit\\POSFileParser\\Tests\\61111042.PU");

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(output, Formatting.Indented, settings);

            File.WriteAllText(@"C:\Users\omgit\source\repos\FuelPOSToolkit\POSFileParser\Tests\trx.json", json);
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(config =>
            {
                config.AddSerilog();
            });
            // Register services from the library
            services.AddTransient<FtpConnection>();
            services.AddSingleton<ILoggerFactory>(x =>
            {
                var providerCollection = x.GetService<LoggerProviderCollection>();
                var factory = new SerilogLoggerFactory(null, true, providerCollection);

                foreach (var provider in x.GetServices<ILoggerProvider>())
                {
                    factory.AddProvider(provider);
                }

                return factory;
            });
        }

        public static void DebuggerTests()
        {
            DebugFileCreator debugger = new DebugFileCreator();
            List<string> processes = new List<string>
            {
                "HTEC GEMPAY 1,S R X",
                "OPT,X"
            };

            List<string> output = debugger.GenerateFileString(processes, 24);

            foreach (var line in output)
            {
                Console.WriteLine(line);
            }

            debugger.CreateFile(output, @"C:\Users\omgit\source\repos\FuelPOSToolkit\FuelPOSDebugTools\");
        }

        public static void FTPTests(FtpConnection ftpService)
        {
            // FtpConnection ftp = new FtpConnection("brickscrap.cloud.seedboxes.cc", "brickscrap", "Hosted19", 9979);

            using (ftpService)
            {
                try
                {
                    ftpService.OpenSession();
                    var files = ftpService.ListDirectory("/files/downloads/Cities.Skylines-CODEX");
                    ftpService.DownloadFiles("/files/downloads/Cities.Skylines-CODEX/", "codex-cities.skylines.00",
                        @"C:\Users\omgit\source\repos\FuelPOSToolkit\FuelPOSToolkitCmdLineUI\", false);
                    foreach (var file in files)
                    {
                        Console.WriteLine(file.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
