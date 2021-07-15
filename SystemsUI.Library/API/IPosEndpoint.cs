using System.Collections.Generic;
using System.Threading.Tasks;
using SystemsUI.Library.Models;

namespace SystemsUI.Library.API
{
    public interface IPosEndpoint
    {
        Task<List<PosModel>> GetAllByStationId(string stationId);
    }
}