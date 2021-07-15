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

        public async Task<List<StationDbModel>> GetAllStations()
        {
            var output = await _db.LoadDataAsync<StationDbModel, dynamic>(StoredProcedures.Stations.GetAll, new { });

            return output;
        }

        public async Task<StationDbModel> GetStationByID(string stationId)
        {
            var output = await _db.LoadDataAsync<StationDbModel, dynamic>(StoredProcedures.Stations.GetById, new { Id = stationId });

            return output.FirstOrDefault();
        }

        public async Task<StationDbModel> AddStation(StationModel station)
        {
            StationDbModel output = new();

            try
            {
                _db.StartTransaction();

                await _db.SaveDataInTransactionAsync(StoredProcedures.Stations.Insert, station);

                var newStation = await _db.LoadDataInTransactionAsync<StationDbModel, dynamic>(StoredProcedures.Stations.GetByKimoceId,
                                                                                               new { station.KimoceId });

                if (newStation is not null)
                {
                    output = newStation.FirstOrDefault();
                }

                _db.CommitTransaction();
            }
            catch
            {
                _db.RollbackTransaction();
            }

            return output;
        }
    }
}
