using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.Commands;
using TsgSystemsToolkit.DataManager.DataAccess;
using TsgSystemsToolkit.DataManager.Models;

namespace TsgSystemsToolkit.DataManager.Handlers
{
    public class InsertStationHandler : IRequestHandler<InsertStationCommand, StationDbModel>
    {
        private readonly IStationData _stationData;

        public InsertStationHandler(IStationData stationData)
        {
            _stationData = stationData;
        }

        public async Task<StationDbModel> Handle(InsertStationCommand request, CancellationToken cancellationToken)
        {
            return await _stationData.AddStation(request.Station);
        }
    }
}
