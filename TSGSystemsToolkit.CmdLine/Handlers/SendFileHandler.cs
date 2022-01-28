using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.GraphQL;
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
                FileAttributes attr = File.GetAttributes(options.FilePath);

                if (attr == FileAttributes.Directory)
                    throw new FileNotFoundException($"{options.FilePath} is a directory, please provide the path to a file.");
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
                _logger.LogError("File or directory not found: {Message}", ex.Message);
                return -1;
            }

            // Try to log in to API
            //      Update token in AppSettings

            // Determine cluster or site list CSV
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
                    Console.WriteLine("0");
                }
                else
                {
                    foreach (var item in result.Data.Station)
                    {
                        Console.WriteLine(item.Name);
                    }
                }
            }
            //      Validate cluster via API call

            //      Validate site list CSV

            // Get list of FTP credentials from API

            // Connect to each station (can we do multiple at a time?)
            //      Send file
            //      Validate file has been delivered successfully

            return 0;
        }
    }
}
