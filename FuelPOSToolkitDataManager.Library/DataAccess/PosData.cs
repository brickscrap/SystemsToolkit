using FuelPOSToolkitDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitLibrary.Models;

namespace FuelPOSToolkitDataManager.Library.DataAccess
{
    public class PosData : IPosData
    {
        private readonly ISqlDataAccess _db;

        public PosData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task AddPOSData(string stationId, List<PCInfoModel> posModels)
        {
            // TODO: Check if POS already exists, update/don't if required
            // TODO: Make this more DRY/SRP
            foreach (var pos in posModels)
            {
                await _db.SaveDataAsync("dbo.spPOSInsert", 
                    new 
                    {
                        StationId = stationId,
                        Number = pos.Number,
                        Type = pos.Type,
                        OperatingSystem = pos.OperatingSystem,
                        HardwareType = pos.HardwareType,
                        SoftwareVersion = pos.SoftwareVersion,
                        PrimaryIP = pos.PrimaryIP,
                        ReceiptPrinter = pos.ReceiptPrinter,
                        CustomerDisplay = pos.CustomerDisplay,
                        BarcodeScanner = pos.BarcodeScanner,
                        LevelGauge = pos.LevelGauge,
                        TouchScreenType = pos.TouchScreenType,
                        UPS = pos.UPS,
                        NumSerialPorts = pos.NumSerialPorts
                    });

                var posIDs = await _db.LoadDataAsync<int, dynamic>("dbo.spGetPOSIdByNumber",
                        new { StationId = stationId, POSNumber = pos.Number });

                int posID = posIDs.FirstOrDefault();

                foreach (var serialDevice in pos.SerialDevices)
                {

                    await _db.SaveDataAsync("dbo.spSerialDeviceInsert",
                        new 
                        {
                            POSHardwareId = posID,
                            Device = serialDevice.Device,
                            PortNumber = serialDevice.PortNumber
                        });
                }
            }

        }
    }
}
