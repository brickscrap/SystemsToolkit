using Spectre.Console;
using StrawberryShake;
using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.GraphQL;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public class AddStationsHandler : ICommandHandler
    {
        private readonly AddStationsOptions _options;
        private readonly CancellationToken _ct;
        private SysTkApiClient _apiClient;

        public AddStationsHandler(AddStationsOptions options, CancellationToken ct = default)
        {
            _options = options;
            _ct = ct;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            GetDependencies(context);

            if (_options.Individual is not null)
            {
                await HandleIndividualStation(_options.Individual);
            }

            if (_options.File is not null)
            {
                // TODO: Validate file contents
                var siteList = File.ReadAllLines(_options.File);

                foreach (var site in siteList)
                {
                    await HandleIndividualStation(site);
                }
            }

            return 0;
        }

        private async Task HandleIndividualStation(string stationLine)
        {
            string[] stationDetails = stationLine.Split(';');
            var ftpCreds = new Uri(stationDetails[3]);
            var userInfo = ftpCreds.UserInfo.Split(':');
            var cluster = (Cluster)Enum.Parse(typeof(Cluster), stationDetails[1]);

            AddStationInput input = new()
            {
                Cluster = cluster,
                Id = stationDetails[0],
                Name = stationDetails[2],
                Ip = ftpCreds.Host
            };

            var stationResult = await _apiClient.AddStation.ExecuteAsync(input, _ct);
            stationResult.EnsureNoErrors();

            if (stationResult.IsSuccessResult() && stationResult.Data.AddStation.Station is not null)
            {
                AnsiConsole.MarkupLine($"Station {stationResult.Data.AddStation.Station.Id} - {stationResult.Data.AddStation.Station.Name} [green]added successfully[/].");

                await AddFtpCredentials(userInfo[0], userInfo[1], input.Id);
            }
            else if (stationResult.Data.AddStation.Errors.Count > 0)
            {
                foreach (var error in stationResult.Data.AddStation.Errors)
                {
                    switch (error)
                    {
                        case AddStation_AddStation_Errors_StationExistsError stationExists:
                            AnsiConsole.MarkupLine($"[red]Error:[/] {stationExists.Message}");
                            await AddFtpCredentials(userInfo[0], userInfo[1], input.Id);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private async Task AddFtpCredentials(string username, string password, string stationId)
        {
            AddFtpCredentialsInput creds = new()
            {
                Username = username,
                Password = password,
                StationId = stationId
            };

            var credsResult = await _apiClient.AddCredentials.ExecuteAsync(creds, _ct);
            credsResult.EnsureNoErrors();

            if (credsResult.IsSuccessResult() && credsResult.Data.AddFtpCredentials.FtpCredentials is not null)
            {
                AnsiConsole.MarkupLine($"FTP credentials for station {creds.StationId} [green]added successfully[/].");
            }
            else if (credsResult.Data.AddFtpCredentials.Errors.Count > 0)
            {
                foreach (var error in credsResult.Data.AddFtpCredentials.Errors)
                {
                    switch (error)
                    {
                        case AddCredentials_AddFtpCredentials_Errors_StationNotExistsError stationNotExists:
                            Console.WriteLine();
                            AnsiConsole.MarkupLine($"[red]Error:[/] {stationNotExists.Message}");
                            break;
                        case AddCredentials_AddFtpCredentials_Errors_FtpCredentialsExistsError credentialsExists:
                            Console.WriteLine();
                            AnsiConsole.MarkupLine($"[red]Error:[/] {credentialsExists.Message}");
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void GetDependencies(InvocationContext context)
        {
            _apiClient = (SysTkApiClient)context.BindingContext.GetService(typeof(SysTkApiClient));
        }
    }
}

// R2798;RontecUK;Billinghurst;FTP://SUPERVISOR:DF572BBC9@10.68.25.21