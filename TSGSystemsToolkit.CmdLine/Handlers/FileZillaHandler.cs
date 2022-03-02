using StrawberryShake;
using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.GraphQL;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
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
            var result = await _apiClient.GetAllStationsAndCredentials.ExecuteAsync(_ct);

            result.EnsureNoErrors();

            if (result.IsSuccessResult() && result.Data.Station is not null)
            {
                var stations = result.Data.Station.ToList();

                foreach (var station in stations)
                {
                    
                }
            }

            return 0;
        }
    }
}
