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

        public async Task<List<POSModel>> GetPOSByStationId(string stationId)
        {
            List<POSModel> output = new List<POSModel>();

            var pos = await _db.LoadDataAsync<POSDbModel, dynamic>("dbo.spGetPOSByStationId", 
                new { StationId = stationId });

            foreach (var p in pos)
            {
                POSModel posModel = new POSModel
                {
                    Type = p.Type,
                    OperatingSystem = p.OperatingSystem,
                    Number = p.Number,
                    HardwareType = p.HardwareType,
                    SoftwareVersion = p.SoftwareVersion,
                    PrimaryIP = p.PrimaryIP,
                    ReceiptPrinter = p.ReceiptPrinter,
                    CustomerDisplay = p.CustomerDisplay,
                    BarcodeScanner = p.BarcodeScanner,
                    LevelGauge = p.LevelGauge,
                    TouchScreen = p.TouchScreenType,
                    NumSerialPorts = p.NumSerialPorts,
                    UPS = p.UPS,
                };

                List<Models.SerialDeviceModel> serialDevices = await _db.LoadDataAsync<Models.SerialDeviceModel, dynamic>("dbo.spGetSerialDevicesByPOSId",
                    new { Id = p.Id });
                posModel.SerialDevices = serialDevices;

                output.Add(posModel);
            }

            return output;
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
