using System.Collections.Generic;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.Models;

namespace TsgSystemsToolkit.DataManager.DataAccess
{
    public interface IStationData
    {
        Task<StationDbModel> AddStation(StationModel station);
        Task<List<StationDbModel>> GetAllStations();
        Task<StationDbModel> GetStationByID(string stationId);
    }
}