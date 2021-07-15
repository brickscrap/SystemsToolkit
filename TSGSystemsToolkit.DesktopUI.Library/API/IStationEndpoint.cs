using System;
using System.Collections.Generic;
using System.Net.Http.Handlers;
using System.Threading.Tasks;
using TSGSystemsToolkit.DesktopUI.Library.Models;

namespace TSGSystemsToolkit.DesktopUI.Library.API
{
    public interface IStationEndpoint
    {
        Task<List<StationModel>> GetAll();
    }
}