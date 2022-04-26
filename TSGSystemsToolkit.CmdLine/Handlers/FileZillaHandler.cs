using StrawberryShake;
using System.Collections.Generic;
using SysTk.Utils;
using SysTk.Utils.FileZilla;
using TSGSystemsToolkit.CmdLine.GraphQL;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers;

internal class FileZillaHandler : ICommandHandler
{
    private readonly FileZillaOptions _options;
    private readonly CancellationToken _ct;
    private SysTkApiClient _apiClient;

    public FileZillaHandler(FileZillaOptions options, CancellationToken ct = default)
    {
        _options = options;
        _ct = ct;
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        GetDependencies(context);

        var result = await _apiClient.GetAllStationsAndCredentials.ExecuteAsync(_ct);

        result.EnsureNoErrors();

        if (result.IsSuccessResult() && result.Data.Station is not null)
        {
            var stations = result.Data.Station.ToList();
            List<HostModel> hosts = new();

            foreach (var station in stations)
            {
                foreach (var creds in station.FtpCredentials)
                {
                    HostModel host = new()
                    {
                        Cluster = station.Cluster.ToString(),
                        Id = station.Id,
                        Ip = station.Ip,
                        Name = station.Name,
                        Username = creds.Username,
                        Password = creds.Password
                    };

                    hosts.Add(host);
                }
            }

            // TODO: Validate path of sitemanager.xml
            if (string.IsNullOrWhiteSpace(_options.SiteManagerPath))
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                _options.SiteManagerPath = $@"{appData}\FileZilla\sitemanager.xml";
            }

            FileZillaSiteManagerCreator.Run(hosts, _options.SiteManagerPath);
        }

        return 0;
    }

    private void GetDependencies(InvocationContext context)
    {
        _apiClient = (SysTkApiClient)context.BindingContext.GetService(typeof(SysTkApiClient));
    }
}
