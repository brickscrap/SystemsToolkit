using FuelPOS.StatDevParser.Helpers;
using FuelPOS.StatDevParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FuelPOS.StatDevParser
{
    public class StatDevParser : IStatDevParser
    {
        private IEnumerable<XElement> _statDev;

        public StatDevParser()
        {

        }

        public StatDevParser(XDocument xmlDoc)
        {
            _statDev = xmlDoc.Elements("ROOT")
                .Elements("TREE")
                .Elements("Device");
        }

        public StatdevModel Parse(XDocument xmlDoc)
        {
            _statDev = xmlDoc.Elements("ROOT")
                .Elements("TREE")
                .Elements("Device");

            StatdevModel output = new()
            {
                StationInfo = GetStationInfo()
            };

            output = GetPOSInfo(output);

            return output;
        }

        private StationInfoModel GetStationInfo()
        {
            return new StationInfoModel
            {
                StationNumber = _statDev.GetStationNumber(),
                StationName = _statDev.GetStationName(),
                Company = _statDev.GetCompany(),
                NumberOfTills = _statDev.GetNumberOfTills()
            };
        }

        private StatdevModel GetPOSInfo(StatdevModel statDev)
        {
            List<PCInfoModel> pcInfoModels = new();
            List<PumpModel> pumpModels = new();

            for (int i = 1; i <= statDev.StationInfo.NumberOfTills; i++)
            {
                List<SerialDeviceModel> serialDevices = GetSerialDevices(i);

                pcInfoModels.Add(GetPeripheralInfo(i, serialDevices));

                var dispensers = _statDev.GetDispenserXML(i).Elements("Device");

                if (dispensers != null)
                {
                    foreach (var dispenser in dispensers)
                    {
                        var pumpNumber = int.Parse(dispenser.Attribute("Number").Value);

                        pumpModels.Add(new PumpModel
                        {
                            Number = pumpNumber,
                            Protocol = _statDev.GetPumpProtocol(i, pumpNumber)
                        });
                    }
                }
            }

            statDev.POS = pcInfoModels;
            statDev.Dispensers = pumpModels;

            return statDev;
        }

        private PCInfoModel GetPeripheralInfo(int pcNumber, List<SerialDeviceModel> serialDevices)
        {
            return new PCInfoModel
            {
                Type = _statDev.GetPCType(pcNumber),
                OperatingSystem = _statDev.GetOS(pcNumber),
                Number = _statDev.GetPCNumber(pcNumber),
                HardwareType = _statDev.GetHardwareType(pcNumber),
                SoftwareVersion = _statDev.GetSoftwareVersion(pcNumber),
                PrimaryIP = _statDev.GetIP(pcNumber),
                ReceiptPrinter = _statDev.GetReceiptPrinter(pcNumber),
                CustomerDisplay = _statDev.GetCustDisplay(pcNumber),
                BarcodeScanner = _statDev.GetBarcodeScanner(pcNumber),
                UPS = _statDev.GetUPS(pcNumber),
                SerialDevices = serialDevices,
                TouchScreenType = _statDev.GetTouchscreenType(pcNumber)
            };
        }

        private List<SerialDeviceModel> GetSerialDevices(int pcNumber)
        {
            List<SerialDeviceModel> output = new();

            foreach (var value in _statDev.GetSerialDevices(pcNumber))
            {
                string portNumber = value.Attribute("Number").Value;
                string device = value.Elements("Property")
                        .Where(elem => (string)elem.Attribute("Type") == "85")
                        .FirstOrDefault()
                        .Value;

                output.Add(new SerialDeviceModel
                {
                    PortNumber = portNumber,
                    Device = device
                });
            }

            return output;
        }
    }
}
