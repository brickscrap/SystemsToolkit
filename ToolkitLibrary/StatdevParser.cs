using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ToolkitLibrary.Models;

namespace ToolkitLibrary
{
    public class StatdevParser : IStatdevParser
    {
        private IEnumerable<XElement> _doc;
        public IEnumerable<XElement> MyProperty { get; set; }
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

            List<PCInfoModel> pcInfoModels = new List<PCInfoModel>();
            List<PumpModel> pumpModels = new List<PumpModel>();

            for (int i = 1; i <= output.StationInfo.NumberOfTills; i++)
            {
                List<SerialDeviceModel> serialDevices = new List<SerialDeviceModel>();

                foreach (var value in GetSerialDevices(i))
                {
                    string portNumber = value.Attribute("Number").Value;
                    string device = value.Elements("Property")
                            .Where(elem => (string)elem.Attribute("Type") == "85")
                            .FirstOrDefault()
                            .Value;

                    serialDevices.Add(new SerialDeviceModel
                    {
                        PortNumber = portNumber,
                        Device = device
                    });
                }

                pcInfoModels.Add(new PCInfoModel
                {
                    Type = GetPCType(i),
                    OperatingSystem = GetOS(i),
                    Number = GetPCNumber(i),
                    HardwareType = GetHardwareType(i),
                    SoftwareVersion = GetSoftwareVersion(i),
                    PrimaryIP = GetIP(i),
                    ReceiptPrinter = GetReceiptPrinter(i),
                    CustomerDisplay = GetCustDisplay(i),
                    BarcodeScanner = GetBarcodeScanner(i),
                    UPS = GetUPS(i),
                    SerialDevices = serialDevices,
                    TouchScreenType = GetTouchscreenType(i)
                });

                var dispensers = GetDispenserXML(i).Elements("Device");

                if (dispensers != null)
                {
                    foreach (var dispenser in dispensers)
                    {
                        var pumpNumber = int.Parse(dispenser.Attribute("Number").Value);

                        pumpModels.Add(new PumpModel
                        {
                            Number = pumpNumber,
                            Protocol = GetPumpProtocol(i, pumpNumber)
                        });
                    }
                }
            }

            output.POS = pcInfoModels;
            output.Dispensers = pumpModels;

            return output;
        }

        #region StationInfo
        private string GetStationNumber()
        {
            var output = _doc.Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "1")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }
        private string GetStationName()
        {
            var output = _doc.Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "2")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }
        private string GetCompany()
        {
            var output = _doc.Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "6")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }
        private int GetNumberOfTills()
        {
            var number = _doc.Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "10")
                .FirstOrDefault();

            if (number != null)
            {
                var output = int.Parse(number.Value);

                return output;
            }

            return 0;
        }
        #endregion

        #region POSInfo
        private string GetPCType(int pcNumber)
        {
            var output = SelectPC(pcNumber)
                .Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "34")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }
        private string GetOS(int pcNumber)
        {
            var output = SelectPC(pcNumber)
                .Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "54")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }
        private int GetPCNumber(int pcNumber)
        {
            var output = SelectPC(pcNumber)
                .Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "68")
                .FirstOrDefault();

            if (output != null)
            {
                return int.Parse(output.Value);
            }

            return 0;
        }
        private string GetHardwareType(int pcNumber)
        {
            var output = SelectPC(pcNumber)
                .Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "147")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }
        private string GetSoftwareVersion(int pcNumber)
        {
            var output = SelectPC(pcNumber)
                .Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "35")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }
        private string GetIP(int pcNumber)
        {
            var output = SelectPC(pcNumber)
                .Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "56")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }
        private string GetReceiptPrinter(int pcNumber)
        {
            try
            {
                var output = SelectPC(pcNumber)
                        .Elements("Device")
                        .Where(item => (string)item.Attribute("Type") == "30")
                        .Elements("Property")
                        .Where(item => (string)item.Attribute("Type") == "34")
                        .FirstOrDefault();

                if (output != null)
                {
                    return output.Value;
                }

                return "None/unknown";
            }
            catch (Exception)
            {
                return null;
            }
        }
        private string GetCustDisplay(int pcNumber)
        {
            try
            {
                var output = SelectPC(pcNumber)
                        .Elements("Device")
                        .Where(item => (string)item.Attribute("Type") == "27")
                        .Elements("Property")
                        .Where(item => (string)item.Attribute("Type") == "34")
                        .FirstOrDefault();

                if (output != null)
                {
                    return output.Value;
                }

                return "None/unknown";
            }
            catch (Exception)
            {
                return null;
            }
        }
        private string GetBarcodeScanner(int pcNumber)
        {
            try
            {
                var output = SelectPC(pcNumber)
                .Elements("Device")
                .Where(value => (string)value.Attribute("Type") == "26")
                .Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "34")
                .FirstOrDefault();

                if (output != null)
                {
                    return output.Value;
                }

                return "None/unknown";
            }
            catch (Exception)
            {
                return null;
            }

        }
        private IEnumerable<XElement> GetSerialDevices(int pcNumber)
        {
            var output = SelectPC(pcNumber)
                .Elements("Device")
                .Where(value => (string)value.Attribute("Type") == "38")
                .Where(value => (int)value.Attribute("Number") == pcNumber)
                .Elements("Device")
                .Where(value => (string)value.Attribute("Type") == "39")
                .Where(value => (int)value.Attribute("Number") == pcNumber)
                .Elements("Device")
                .Where(value => (string)value.Attribute("Type") == "41");

            return output;
        }

        private string GetUPS(int pcNumber)
        {
            try
            {
                var output = SelectPC(pcNumber)
                .Elements("Device")
                .Where(value => (string)value.Attribute("Type") == "16")
                .Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "28")
                .FirstOrDefault();

                if (output != null)
                {
                    return output.Value;
                }

                return "None/unknown";
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetTouchscreenType(int pcNumber)
        {
            var output = SelectPC(pcNumber)
                .Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "42")
                .Elements("Device")
                .Where(item => item.Value.Contains("TouchScreenType"))
                .Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "89")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }
        #endregion

        #region DispenserInfo

        private string GetPumpProtocol(int pcNumber, int pumpNumber)
        {
            try
            {
                var output = SelectDispensers(pcNumber)
                .Elements("Device")
                .Where(value => (string)value.Attribute("Type") == "9")
                .Where(value => (int)value.Attribute("Number") == pumpNumber)
                .Elements("Property")
                .Where(value => (int)value.Attribute("Type") == 60)
                .LastOrDefault()
                .Value;

                return output;
            }
            catch (Exception)
            {
                return null;
            }

        }

        #endregion

        #region HelperMethods

        private IEnumerable<XElement> GetDispenserXML(int pcNumber)
        {
            var output = SelectPC(pcNumber)
                    .Elements("Device")
                    .Where(value => (string)value.Attribute("Type") == "8");

            return output;
        }

        private XElement SelectPC(int pcNumber)
        {
            var output = _doc.Elements("Device")
                .Where(value => (string)value.Attribute("Type") == "2" && value.Attribute("Number") != null)
                .Where(number => (int)number.Attribute("Number") == pcNumber)
                .FirstOrDefault();

            return output;
        }
        private XElement SelectDispensers(int pcNumber)
        {
            var output = SelectPC(pcNumber)
                    .Elements("Device")
                    .Where(value => (string)value.Attribute("Type") == "8")
                    .Where(value => (int)value.Attribute("Number") == pcNumber)
                    .FirstOrDefault();

            return output;
        }

        #endregion
    }
}
