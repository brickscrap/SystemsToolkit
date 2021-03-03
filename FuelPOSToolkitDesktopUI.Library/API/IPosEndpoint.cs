using FuelPOSToolkitDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuelPOSToolkitDesktopUI.Library.API
{
    public interface IPosEndpoint
    {
        Task<List<POSModel>> GetAllByStationId(string stationId);
    }
}