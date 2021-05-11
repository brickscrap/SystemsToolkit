using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.DataAccess;
using TsgSystemsToolkit.DataManager.Models;
using TsgSystemsToolkit.DataManager.Queries;

namespace TsgSystemsToolkit.DataManager.Handlers
{
    public class GetStationListHandler : IRequestHandler<GetStationListQuery, List<StationDbModel>>
    {
        private readonly IStationData _stationData;

        public GetStationListHandler(IStationData stationData)
        {
            _stationData = stationData;
        }

        public async Task<List<StationDbModel>> Handle(GetStationListQuery request, CancellationToken cancellationToken)
        {
            var output = await _stationData.GetAllStations();

            return output;
        }
    }
}
