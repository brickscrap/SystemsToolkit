using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ToolkitLibrary.Models;

namespace ToolkitLibrary
{
    public class StatdevParser
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

                foreach (var item in GetSerialDevices(i))
                {
                    int portNumber = int.Parse(item.Attribute("Number").Value);
                    string device = item.Elements("Property")
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
            try
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
            catch (Exception)
            {
                return null;
            }
        }
        private string GetCustDisplay(int pcNumber)
        {
            try
            {
                string output = SelectPC(pcNumber)
                        .Elements("Device")
                        .Where(item => (string)item.Attribute("Type") == "27")
                        .Elements("Property")
                        .Where(item => (string)item.Attribute("Type") == "34")
                        .FirstOrDefault()
                        .Value;

                return output;
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
                string output = SelectPC(pcNumber)
                .Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "26")
                .Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "34")
                .FirstOrDefault()
                .Value;

                return output;
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
                .Where(item => (string)item.Attribute("Type") == "38")
                .Where(item => (int)item.Attribute("Number") == pcNumber)
                .Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "39")
                .Where(item => (int)item.Attribute("Number") == pcNumber)
                .Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "41");

            return output;
        }

        private string GetUPS(int pcNumber)
        {
            try
            {
                string output = SelectPC(pcNumber)
                .Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "16")
                .Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "28")
                .FirstOrDefault()
                .Value;

                return output;
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
                .FirstOrDefault()
                .Value;

            return output;
        }
        #endregion

        #region DispenserInfo

        private string GetPumpProtocol(int pcNumber, int pumpNumber)
        {
            try
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
                    .Where(item => (string)item.Attribute("Type") == "8");

            return output;
        }

        private XElement SelectPC(int pcNumber)
        {
            var output = _doc.Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "2" && item.Attribute("Number") != null)
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
    }
}
