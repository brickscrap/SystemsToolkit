using FuelPOSToolkitDataManager.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuelPOSToolkitDataManager.Library.DataAccess
{
    public interface IStationData
    {
        Task AddStation(StationModel station);
        Task<List<StationModel>> GetAllStations();
        Task<StationModel> GetStationByID(string stationId);
    }
}