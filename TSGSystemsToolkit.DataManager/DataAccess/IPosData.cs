﻿using FuelPOS.StatDevParser.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.Models;

namespace TsgSystemsToolkit.DataManager.DataAccess
{
    public interface IPosData
    {
        Task AddPOSData(string stationId, List<PCInfoModel> posModels);
        Task<List<POSModel>> GetPOSByStationId(string stationId);
    }
}