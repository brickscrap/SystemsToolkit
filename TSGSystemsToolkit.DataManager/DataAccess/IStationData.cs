using System.Collections.Generic;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.Models;

namespace TsgSystemsToolkit.DataManager.DataAccess
{
    public interface IStationData
    {
        Task AddStation(StationModel station);
        Task<List<StationModel>> GetAllStations();
        Task<StationModel> GetStationByID(string stationId);
    }
}