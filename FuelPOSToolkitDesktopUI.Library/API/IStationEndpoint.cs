using FuelPOSToolkitDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Handlers;
using System.Threading.Tasks;

namespace FuelPOSToolkitDesktopUI.Library.API
{
    public interface IStationEndpoint
    {
        Task<List<StationModel>> GetAll();
    }
}