using Grpc.Core;
using GrpcServer;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TsgSystems.Api.Services
{
    public class FpDebugService : FpDebug.FpDebugBase
    {
        private readonly ILogger<FpDebugService> _logger;

        public FpDebugService(ILogger<FpDebugService> logger)
        {
            _logger = logger;
        }

        public override Task<FpDebugReply> DebugFuelPOS(FpDebugRequest request, ServerCallContext context)
        {
            _logger.LogInformation("{StationId}, {Proccesses}", request.StationId, request.Processes);
            System.Console.WriteLine(request.StationId + " " + request.Processes);

            return Task.FromResult(new FpDebugReply { StatusCode = "200" });
        }
    }
}
