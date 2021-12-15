using System.Collections.Generic;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.Models;

namespace TsgSystemsToolkit.DataManager.DataAccess
{
    public interface IPosData
    {
        Task AddPOSData(string stationId, List<POSModel> posModels);
        Task<List<POSModel>> GetPOSByStationId(string stationId);
    }
}