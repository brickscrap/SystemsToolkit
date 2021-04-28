using System.Collections.Generic;
using System.Threading.Tasks;
using SystemsUI.Library.Models;

namespace SystemsUI.Library.API
{
    public interface IStationEndpoint
    {
        Task<List<StationModel>> GetAll();
    }
}