using FluentFTP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShellProgressBar;
using Spectre.Console;
using StrawberryShake;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SysTk.DataManager.Ftp;
using TSGSystemsToolkit.CmdLine.Constants;
using TSGSystemsToolkit.CmdLine.GraphQL;
using TSGSystemsToolkit.CmdLine.Models;
using TSGSystemsToolkit.CmdLine.Options;
using TSGSystemsToolkit.CmdLine.Services;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    internal class SendFileHandler : AbstractHandler<SendFileOptions>
    {
        private readonly SysTkApiClient _apiClient;
        private readonly ILogger<SendFileHandler> _logger;
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;
        private readonly IFtpService _ftpService;

        public SendFileHandler(SysTkApiClient apiClient, ILogger<SendFileHandler> logger, IConfiguration config,
            IAuthService authService, IFtpService ftpService)
        {
            _apiClient = apiClient;
            _logger = logger;
            _config = config;
            _authService = authService;
            _ftpService = ftpService;
        }

        public override async Task<int> RunHandlerAndReturnExitCode(SendFileOptions options, CancellationToken ct = default(CancellationToken))
        {
            // Validate Options
            if (!ValidateOptions(options))
                return -1;

            // Add all transfers to this list
            List<FtpTransferModel> transfers = new();

            // Get cluster stations first
            if (options.Cluster is not null)
            {
                string cluster = options.Cluster.Replace(" ", "");
                var result = await _apiClient.GetStationsByCluster.ExecuteAsync(cluster, ct);

                try
                {
                    result.EnsureNoErrors();
                }
                catch (GraphQLClientException ex)
                {
                    return await HandleGraphQLExceptions<IGetStationsByClusterResult>(ex, result);
                }
            }

            // Get sites from list file
            if (options.List is not null)
            {
                var listFile = File.ReadAllLines(options.List).ToList();

                foreach (var id in listFile)
                {
                    var result = await _apiClient.GetCredentialsByStationId.ExecuteAsync(id);
                }
            }

            // Get individual site
            if (options.Site is not null)
            {

            }

            ThreadPool.GetMaxThreads(out int maxThreads, out int completionPortThreads);
            ThreadPool.SetMaxThreads(5, 5);

            var creds = new List<dynamic>
            {
                new { IP = "185.149.90.82", Username = "brickscrap", Password = "4uUiEUMVXJTb", Local = @"C:\Users\GaryM\Downloads\VSCodeUserSetup-x64-1.63.2.exe", Target = "/poller_test/VSCodeUserSetup-x64-1.63.2.exe", Port = 9979},
                //new { IP = "185.149.90.82", Username = "brickscrap", Password = "4uUiEUMVXJTb", Local = @"C:\Users\GaryM\Downloads\go1.17.6.windows-amd64.msi", Target = "/poller_test/go1.17.6.windows-amd64.msi", Port = 9979},
                new { IP = "185.149.90.82", Username = "brickscrap", Password = "4uUiEUMVXJTb", Local = @"C:\Users\GaryM\Downloads\Insomnia.Core-2021.7.2.exe", Target = "/poller_test/Insomnia.Core-2021.7.2.exe", Port = 9979}
            };

            List<Task> tasks = new();

            AnsiConsole.Progress()
                .AutoClear(true)
                .Columns(new ProgressColumn[]
            {
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new RemainingTimeColumn(),
                    new SpinnerColumn()
            })
            .Start(ctx =>
            {
                foreach (var stat in creds)
                {
                    Task task = Task.Factory.StartNew((x) =>
                    {
                        try
                        {
                            _logger.LogInformation("Uploading...");

                            using var ftpClient = new FtpClient(stat.IP, stat.Port, stat.Username, stat.Password);
                            ftpClient.Connect();
                            ProgressTask progTask = ctx.AddTask(stat.Local, maxValue: 100, autoStart: true);

                            double increment = 0;
                            double lastProg = 0;
                            Action<FtpProgress> prog = x =>
                            {
                                if (increment == 0)
                                {
                                    increment = x.Progress;
                                    lastProg = x.Progress;
                                } 
                                else
                                {
                                    increment = x.Progress - lastProg;
                                    lastProg = x.Progress;
                                }

                                progTask.Increment(increment);
                            };

                            ftpClient.UploadFile(stat.Local, stat.Target, FtpRemoteExists.Overwrite, progress: prog);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("Inside Task: {Message}", ex.Message);
                            _logger.LogError("{Inner}", ex.InnerException);
                            _logger.LogError("{Trace}", ex.StackTrace);
                        }
                    }, TaskCreationOptions.AttachedToParent, cancellationToken: ct);

                    tasks.Add(task);
                }
                try
                {
                    Task.WaitAll(tasks.ToArray(), ct);

                }
                catch (Exception ex)
                {
                    _logger.LogError("Errrrror: {Error}", ex.Message);
                }
            });
            return 0;
            // END TEST CODE


            // Try to log in to API
            //      Update token in AppSettings

            // Determine cluster or site list CSV

            try
            {
                /*
                if (options.Cluster is not null)
                {
                    string cluster = options.Cluster.Replace(" ", "");
                    var result = await _apiClient.GetStationsByCluster.ExecuteAsync(cluster);

                    if (result.Errors.Count > 0)
                    {
                        if (result.Errors.Where(x => x.Code == "AUTH_NOT_AUTHENTICATED").Any())
                        {
                            Console.WriteLine("Not authenticated. Current token invalid or expired. Proceed with login process.");
                            var authResult = await _authService.Authenticate();

                            if (authResult)
                            {
                                _logger.LogInformation("Login success. Please re-run your command.");
                                return 0;
                            }
                        }
                    }

                    
                    if (result.Data.Station.Count == 0)
                    {
                        _logger.LogError("Could not find any stations in cluster: {cluster}", options.Cluster);
                        _logger.LogDebug("Input: {InputCluster}; Transform: {TransformCluster}", options.Cluster, cluster);
                        return -1;
                    }
                    else
                    {
                        ThreadPool.GetMaxThreads(out int maxThreads, out int completionPortThreads);
                        ThreadPool.SetMaxThreads(5, 5);

                        var stations = result.Data.Station;


                        List<Task> tasks = new();

                        foreach (var stat in stations)
                        {
                            Task task = Task.Factory.StartNew(async (x) =>
                            {
                                try
                                {
                                    var success = await _ftpService.UploadFileAsync(stat.IP, stat.FtpCredentials[0].Username, stat.FtpCredentials[0].Password, options.FilePath, options.Target, overwrite: true, ct: ct);

                                    if (success)
                                    {
                                        _logger.LogInformation("Upload of {File} to {StationID} - {StationName} success!", options.FilePath, stat.Name, stat.IP);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError("Could not upload file to {StationId} - {StationName}: {Message}", stat.Id, stat.Name, ex.Message);
                                }
                            }, TaskCreationOptions.AttachedToParent, cancellationToken: ct);

                            tasks.Add(task);
                        }

                        Task.WaitAll(tasks.ToArray(), ct);

                        //foreach (var item in result.Data.Station)
                        //{
                        //    // Connect to station
                            
                        //    // Send file
                        //}
                    }
                }
                */
                if (options.List is not null)
                {

                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogCritical("Error connecting to API: {Message}", ex.Message);
                _logger.LogDebug("Inner Exception: {Inner}", ex.InnerException);
                _logger.LogDebug("Trace: {StackTrace}", ex.StackTrace);
            }


            //      Validate cluster via API call

            //      Validate site list CSV

            // Get list of FTP credentials from API

            // Connect to each station (can we do multiple at a time?)
            //      Send file
            //      Validate file has been delivered successfully

            return 0;
        }

        private async Task<int> HandleGraphQLExceptions<T>(GraphQLClientException exception, IOperationResult<T> operationResult) where T : class
        {
            _logger.LogError("Error from API: {Message}", exception.Message);
            if (operationResult.Errors.Where(x => x.Code == ApiErrorCodes.NotAuthenticated).Any())
            {
                bool authSuccess = await HandleNotAuthenticated();
                if (authSuccess)
                    return 0;
                else
                    return 1;
            }

            return 1;
        }

        private async Task<bool> HandleNotAuthenticated()
        {
            AnsiConsole.WriteLine("[bold red]Not authenticated.[/] Current token invalid or expired. Proceed with login process.");
            var authResult = await _authService.Authenticate();

            if (authResult)
            {
                AnsiConsole.WriteLine("[bold green]Login success.[/] Please re-run your command.");
                return true;
            }
            else
            {
                AnsiConsole.WriteLine("[bold red]Authentication failed.[/]");
                return false;
            }
        }

        private bool ValidateOptions(SendFileOptions options)
        {
            try
            {
                if (IsDirectory(options.FilePath))
                    throw new FileNotFoundException($"{options.FilePath} is a directory, please provide the path to a file.");

                if (options.List is not null && IsDirectory(options.List))
                    throw new FileNotFoundException($"{options.List} is a directory, please provide the path to a CSV or text file containing a list of station IDs.");
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
                _logger.LogError("File or directory not found: {Message}", ex.Message);
                return false;
            }

            return true;
        }

        private bool IsDirectory(string path)
        {
            FileAttributes attr = File.GetAttributes(path);

            if (attr == FileAttributes.Directory)
                return true;

            return false;
        }
    }
}
