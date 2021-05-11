using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.DataAccess;
using TsgSystemsToolkit.DataManager.Models;
using TsgSystemsToolkit.DataManager.Queries;

namespace TsgSystemsToolkit.DataManager.Handlers
{
    public class GetStationByIdHandler : IRequestHandler<GetStationByIdQuery, StationDbModel>
    {
        private readonly IStationData _stationData;

        public GetStationByIdHandler(IStationData stationData)
        {
            _stationData = stationData;
        }

        public async Task<StationDbModel> Handle(GetStationByIdQuery request, CancellationToken cancellationToken)
        {
            return await _stationData.GetStationByID(request.Id);
        }
    }
}
