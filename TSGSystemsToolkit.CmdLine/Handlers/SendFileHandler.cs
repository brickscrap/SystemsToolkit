using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

        public SendFileHandler(SysTkApiClient apiClient, ILogger<SendFileHandler> logger, IConfiguration config,
            IAuthService authService)
        {
            _apiClient = apiClient;
            _logger = logger;
            _config = config;
            _authService = authService;
        }

        public override async Task<int> RunHandlerAndReturnExitCode(SendFileOptions options)
        {
            // Validate options.FilePath
            try
            {
                if (IsDirectory(options.FilePath))
                    throw new FileNotFoundException($"{options.FilePath} is a directory, please provide the path to a file.");

                if (options.List is not null && IsDirectory(options.List))
                    throw new FileNotFoundException($"{options.List} is a directory, please provide the path to a CSV file containing a list of station IDs.");
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
                _logger.LogError("File or directory not found: {Message}", ex.Message);
                return -1;
            }

            // Try to log in to API
            //      Update token in AppSettings

            // Determine cluster or site list CSV

            try
            {
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

                        foreach (var item in result.Data.Station)
                        {
                            // Connect to station
                            
                            // Send file
                        }
                    }
                }

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

        private bool IsDirectory(string path)
        {
            FileAttributes attr = File.GetAttributes(path);

            if (attr == FileAttributes.Directory)
                return true;

            return false;
        }
    }
}
