using System;
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
            Console.WriteLine("Invoked");
            GetDependencies(context);

            var client = (SysTkApiClient)context.BindingContext.GetService(typeof(SysTkApiClient));

            var stats = await client.GetAllStations.ExecuteAsync();

            return 0;
        }

        private void GetDependencies(InvocationContext context)
        {
            _apiClient = (SysTkApiClient)context.BindingContext.GetService(typeof(SysTkApiClient));
        }
    }
}
