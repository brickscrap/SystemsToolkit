using System.Collections.Generic;
using System.Threading.Tasks;
using TSGSystemsToolkit.DesktopUI.Library.Models;

namespace TSGSystemsToolkit.DesktopUI.Library.API
{
    public interface IPosEndpoint
    {
        Task<List<POSModel>> GetAllByStationId(string stationId);
    }
}