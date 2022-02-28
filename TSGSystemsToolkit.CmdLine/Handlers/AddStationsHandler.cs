using Spectre.Console;
using StrawberryShake;
using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
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
                string[] stationDetails = _options.Individual.Split(';');
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

                var result = await _apiClient.AddStation.ExecuteAsync(input);
                result.EnsureNoErrors();

                if (result.IsSuccessResult() && result.Data.AddStation.Station is not null)
                    AnsiConsole.MarkupLine($"Station {result.Data.AddStation.Station.Id} - {result.Data.AddStation.Station.Name} [green]added successfully[/].");
                else if (result.Data.AddStation.Errors.Count > 0)
                {
                    foreach (var error in result.Data.AddStation.Errors)
                    {
                        switch (error)
                        {
                            case AddStation_AddStation_Errors_StationExistsError stationExists:
                                AnsiConsole.MarkupLine($"[red]Error:[/] {stationExists.Message}");
                                break;
                            default:
                                break;
                        }
                    }
                }

            }

            return 0;
        }

        private void GetDependencies(InvocationContext context)
        {
            _apiClient = (SysTkApiClient)context.BindingContext.GetService(typeof(SysTkApiClient));
        }
    }
}

// R2798;RontecUK;Billinghurst;FTP://SUPERVISOR:DF572BBC9@10.68.25.21