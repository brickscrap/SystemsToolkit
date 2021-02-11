using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using StadevParser.Models;

namespace StadevParser
{
    public class StatdevParser : IStatdevParser
    {
        private IEnumerable<XElement> _doc;
        public StatdevParser()
        {

        }
        public StatdevParser(XDocument xmlDoc)
        {
            _doc = xmlDoc.Elements("ROOT")
                .Elements("TREE")
                .Elements("Device");
        }

        public StatdevModel Parse(XDocument xmlDoc)
        {
            _doc = xmlDoc.Elements("ROOT")
                .Elements("TREE")
                .Elements("Device");

            StatdevModel output = CreateStatdev();

            output = AddAllPCs(output);

            return output;
        }

        public StatdevModel Parse(string xmlDoc)
        {
            var document = XDocument.Parse(xmlDoc);
            _doc = document.Elements("ROOT")
                .Elements("TREE")
                .Elements("Device");

            StatdevModel output = CreateStatdev();

            output = AddAllPCs(output);

            return output;
        }

        private StatdevModel CreateStatdev()
        {
            StatdevModel output = new StatdevModel
            {
                StationInfo = new StationInfoModel
                {
                    StationNumber = GetStationNumber(),
                    StationName = GetStationName(),
                    Company = GetCompany(),
                    NumberOfTills = GetNumberOfTills()
                }
            };

            return output;
        }

        private StatdevModel AddAllPCs(StatdevModel statDev)
        {
            List<PCInfoModel> pcInfoModels = new List<PCInfoModel>();
            List<PumpModel> pumpModels = new List<PumpModel>();

            for (int tillNumber = 1; tillNumber <= statDev.StationInfo.NumberOfTills; tillNumber++)
            {
                pcInfoModels.Add(CreatePCInfoModel(tillNumber));

                pumpModels = AddPumpModels(tillNumber, pumpModels);
            }

            statDev.POS = pcInfoModels;
            statDev.Dispensers = pumpModels;

            return statDev;
        }

        private PCInfoModel CreatePCInfoModel(int pcNumber)
        {
            PCInfoModel output = new PCInfoModel
            {
                Type = GetPCType(pcNumber),
                OperatingSystem = GetOS(pcNumber),
                Number = GetPCNumber(pcNumber),
                HardwareType = GetHardwareType(pcNumber),
                SoftwareVersion = GetSoftwareVersion(pcNumber),
                PrimaryIP = GetIP(pcNumber),
                ReceiptPrinter = GetReceiptPrinter(pcNumber),
                CustomerDisplay = GetCustDisplay(pcNumber),
                BarcodeScanner = GetBarcodeScanner(pcNumber),
                UPS = GetUPS(pcNumber),
                SerialDevices = GetSerialDevices(pcNumber),
                TouchScreen = GetTouchscreenType(pcNumber)
            };

            return output;
        }

        private List<PumpModel> AddPumpModels(int tillNumber, List<PumpModel> pumps)
        {
            var dispensers = GetDispenserXML(tillNumber).Elements("Device");

            if (dispensers != null)
            {
                foreach (var dispenser in dispensers)
                {
                    var pumpNumber = int.Parse(dispenser.Attribute("Number").Value);

                    pumps.Add(new PumpModel
                    {
                        Number = pumpNumber,
                        Protocol = GetPumpProtocol(tillNumber, pumpNumber)
                    });
                }
            }

            return pumps;
        }

        #region StationInfo
        private string GetStationNumber()
        {
            string output = _doc.Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "1")
                .FirstOrDefault()
                .Value;

            return output;
        }
        private string GetStationName()
        {
            string output = _doc.Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "2")
                .FirstOrDefault()
                .Value;

            return output;
        }
        private string GetCompany()
        {
            string output = _doc.Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "6")
                .FirstOrDefault()
                .Value;

            return output;
        }
        private int GetNumberOfTills()
        {
            string number = _doc.Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "10")
                .FirstOrDefault()
                .Value;

            var output = int.Parse(number);

            return output;
        }
        #endregion

        #region POSInfo
        private string GetPCType(int pcNumber)
        {
            string output = SelectPC(pcNumber)
                .Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "34")
                .FirstOrDefault()
                .Value;

            return output;
        }
        private string GetOS(int pcNumber)
        {
            string output = SelectPC(pcNumber)
                .Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "54")
                .FirstOrDefault()
                .Value;

            return output;
        }
        private int GetPCNumber(int pcNumber)
        {
            string output = SelectPC(pcNumber)
                .Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "68")
                .FirstOrDefault()
                .Value;

            return int.Parse(output);
        }
        private string GetHardwareType(int pcNumber)
        {
            string output = SelectPC(pcNumber)
                .Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "147")
                .FirstOrDefault()
                .Value;

            return output;
        }
        private string GetSoftwareVersion(int pcNumber)
        {
            string output = SelectPC(pcNumber)
                .Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "35")
                .FirstOrDefault()
                .Value;

            return output;
        }
        private string GetIP(int pcNumber)
        {
            string output = SelectPC(pcNumber)
                .Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "56")
                .FirstOrDefault()
                .Value;

            return output;
        }
        private string GetReceiptPrinter(int pcNumber)
        {
            string output = SelectPC(pcNumber)
                .Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "30")
                .Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "34")
                .FirstOrDefault()
                .Value;

            return output;
        }
        private string GetCustDisplay(int pcNumber)
        {
            // TODO: Group by USB/Serial
            // Serial = DM202
            string output = SelectPC(pcNumber)
                .Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "27")
                .Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "34")
                .FirstOrDefault()
                .Value;

            return output;
        }
        private string GetBarcodeScanner(int pcNumber)
        {
            // TODO: Group by USB/Serial
            // Serial are Metrologic
            string output = SelectPC(pcNumber)
                .Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "26")
                .Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "34")
                .FirstOrDefault()
                .Value;

            return output;
        }
        private List<SerialDeviceModel> GetSerialDevices(int pcNumber)
        {
            var serialDevices = SelectPC(pcNumber)
                .Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "38")
                .Where(item => (int)item.Attribute("Number") == pcNumber)
                .Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "39")
                .Where(item => (int)item.Attribute("Number") == pcNumber)
                .Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "41");

            List<SerialDeviceModel> output = new List<SerialDeviceModel>();

            foreach (var item in serialDevices)
            {
                string portNumber = item.Attribute("Number").Value;
                string device = item.Elements("Property")
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

        private string GetUPS(int pcNumber)
        {
            // TODO: Group by USB/Serial
            // Serial have a number
            string output = SelectPC(pcNumber)
                .Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "16")
                .Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "28")
                .FirstOrDefault()
                .Value;

            return output;
        }

        private string GetTouchscreenType(int pcNumber)
        {
            //string output = SelectPC(pcNumber)
            //    .Elements("Device")
            //    .Where(item => (string)item.Attribute("Type") == "42")
            //    .Elements("Device")
            //    .Where(item => (string)item.Attribute("Number") == "259")
            //    .Elements("Property")
            //    .Where(item => (string)item.Attribute("Type") == "89")
            //    .FirstOrDefault()
            //    .Value;

            string output = SelectPC(pcNumber)
                .Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "42")
                .Elements("Property")
                .Where(item => (string)item.Value == "TouchScreenType")
                .Where(item => (string)item.Attribute("Type") == "89")
                .FirstOrDefault()
                .Value;

            return output;
        }
        #endregion

        #region DispenserInfo

        private string GetPumpProtocol(int pcNumber, int pumpNumber)
        {
            var output = SelectDispensers(pcNumber)
                .Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "9")
                .Where(item => (int)item.Attribute("Number") == pumpNumber)
                .Elements("Property")
                .Where(item => (int)item.Attribute("Type") == 60)
                .LastOrDefault()
                .Value;

            return output;
        }

        #endregion

        #region HelperMethods

        private IEnumerable<XElement> GetDispenserXML(int pcNumber)
        {
            var output = SelectPC(pcNumber)
                    .Elements("Device")
                    .Where(item => (string)item.Attribute("Type") == "8");

            return output;
        }

        private XElement SelectPC(int pcNumber)
        {
            var output = _doc.Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "2")
                .Where(number => (int)number.Attribute("Number") == pcNumber)
                .FirstOrDefault();

            return output;
        }
        private XElement SelectDispensers(int pcNumber)
        {
            var output = SelectPC(pcNumber)
                    .Elements("Device")
                    .Where(item => (string)item.Attribute("Type") == "8")
                    .Where(item => (int)item.Attribute("Number") == pcNumber)
                    .FirstOrDefault();

            return output;
        }

        #endregion

        public string GetTouchScreenTest()
        {
            return GetTouchscreenType(1);
        }
    }
}
