using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.Constants;
using TsgSystemsToolkit.DataManager.Models;

namespace TsgSystemsToolkit.DataManager.DataAccess
{
    public class StationData : IStationData
    {
        private readonly ISqlDataAccess _db;

        public StationData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<List<StationModel>> GetAllStations()
        {
            var output = await _db.LoadDataAsync<StationModel, dynamic>(StoredProcedures.Stations_GetAll, new { });

            return output;
        }

        public async Task<StationModel> GetStationByID(string stationId)
        {
            var output = await _db.LoadDataAsync<StationModel, dynamic>(StoredProcedures.Stations_GetById, new { Id = stationId });

            return output.FirstOrDefault();
        }

        public async Task AddStation(StationModel station)
        {
            await _db.SaveDataAsync(StoredProcedures.Stations_Insert, station);
        }
    }
}
