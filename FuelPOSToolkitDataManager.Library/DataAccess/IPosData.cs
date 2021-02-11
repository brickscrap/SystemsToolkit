using FuelPOSToolkitDataManager.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToolkitLibrary.Models;

namespace FuelPOSToolkitDataManager.Library.DataAccess
{
    public interface IPosData
    {
        Task AddPOSData(string stationId, List<PCInfoModel> posModels);
    }
}