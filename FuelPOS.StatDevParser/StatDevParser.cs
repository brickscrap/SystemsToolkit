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
                StationInfo = GetStationInfo(xmlDoc)
            };

            output = GetPOSInfo(xmlDoc, output);

            return output;
        }

        private StationInfoModel GetStationInfo(XDocument xmlDoc)
        {
            var doc = xmlDoc.Elements("ROOT")
                .Elements("TREE")
                .Elements("Device");

            // Get station info XML
            var stationInfo = doc.Elements().Where(x => x.Name == "Property");

            StationInfoModel output = new();

            // Assign values
            foreach (var item in stationInfo)
            {
                switch (item.Attribute("Type").Value)
                {
                    case "1":
                        Console.WriteLine($"Station number: {item.Value}");
                        output.StationNumber = item.Value;
                        break;
                    case "2":
                        Console.WriteLine($"Station name: {item.Value}");
                        output.StationName = item.Value;
                        break;
                    case "3":
                        output.Address = item.Value;
                        break;
                    case "4":
                        output.Place = item.Value;
                        break;
                    case "5":
                        output.Postcode = item.Value;
                        break;
                    case "6":
                        output.Company = item.Value;
                        break;
                    case "10":
                        output.NumberOfPos = int.Parse(item.Value);
                        break;
                    case "63":
                        output.StationIP = item.Value;
                        break;
                    case "146":
                        output.LogoName = item.Value;
                        break;
                    default:
                        break;
                }
            }

            return output;
        }

        private StatdevModel GetPOSInfo(XDocument xmlDoc, StatdevModel statDev)
        {
            List<PosDetailModel> posList = new();

            var doc = xmlDoc.Elements("ROOT")
                .Elements("TREE")
                .Elements("Device");

            // Get all POS devices
            var posInfo = doc.Elements("Device").Where(x => x.Attribute("Type").Value == "2");

            foreach (var pos in posInfo)
            {
                PosDetailModel posDetail = new();
                // Get POS base properties
                var baseProperties = pos.Elements("Property");

                // Assign base properties
                foreach (var bP in baseProperties)
                {
                    switch (bP.Attribute("Type").Value)
                    {
                        case "34":
                            posDetail.Type = bP.Value;
                            break;
                        case "56":
                            posDetail.PrimaryIP = bP.Value;
                            break;
                        case "64":
                            posDetail.GatewayIP = bP.Value;
                            break;
                        case "54":
                            posDetail.OperatingSystem = bP.Value;
                            break;
                        case "68":
                            posDetail.Number = int.Parse(bP.Value);
                            break;
                        case "147":
                            posDetail.HardwareType = bP.Value;
                            break;
                        case "35":
                            posDetail.SoftwareVersion = bP.Value;
                            break;
                        default:
                            break;
                    }
                }

                // Get all POS Devices
                var posDevices = pos.Elements("Device");

                foreach (var dev in posDevices)
                {
                    switch (dev.Attribute("Type").Value)
                    {
                        case "8":
                            posDetail.Dispensing = GetDispensing(dev);
                            break;
                        case "11":
                            statDev.TankManagement = GetTankManagement(dev);
                            break;
                        case "16":
                            string ups = dev.GetValue("Property", "Type", "28");
                            if (int.TryParse(ups, out _))
                                posDetail.UPS = "Serial";
                            else
                                posDetail.UPS = ups;
                            break;
                        case "18":
                            posDetail.A4Printer = dev.GetPropType34();
                            break;
                        case "26":
                            posDetail.BarcodeScanner = dev.GetPropType34();
                            break;
                        case "27":
                            posDetail.CustomerDisplay = dev.GetPropType34();
                            break;
                        case "30":
                            posDetail.ReceiptPrinter = dev.GetPropType34();
                            break;
                        case "31":
                            posDetail.MagCardReader = dev.GetPropType34();
                            break;
                        case "32":
                            if (dev.Attribute("Number").Value == "1")
                                posDetail.PaymentTerminal = dev.GetPropType34();
                            break;
                        case "33":
                            if (dev.Attribute("Number").Value == "1")
                                posDetail.LoyaltyTerminal = dev.GetPropType34();
                            break;
                        case "38":
                            posDetail.ComDevices = GetComDevices(dev);
                            break;
                        case "42":
                            posDetail.TouchScreenType = GetTouchScreen(dev);
                            break;
                        case "95":
                            if (dev.Attribute("Number").Value == "1")
                                posDetail.BuildDisk = dev.GetValue("Property", "Type", "41");
                            break;
                        default:
                            break;
                    }
                }

                statDev.POS.Add(posDetail);
            }

            return statDev;
        }

        private string GetTouchScreen(XElement xml)
        {
            var screen = xml.Elements("Device")
                .Where(x => x.Value.Contains("TouchScreenType"));

            var output = screen.Elements("Property")
                .Where(x => x.Attribute("Type").Value == "89")
                .FirstOrDefault()
                .Value;

            return output;
        }

        private List<DispensingModel> GetDispensing(XElement xml)
        {
            List<DispensingModel> output = new();

            foreach (var disp in xml.Elements("Device"))
            {
                DispensingModel dispenser = new();
                dispenser.Number = int.Parse(disp.Attribute("Number").Value);

                foreach (var prop in disp.Elements("Property"))
                {
                    if (prop.Attribute("Type").Value == "60")
                        dispenser.Protocol = prop.Value;
                }

                output.Add(dispenser);
            }


            return output;
        }

        private ComDeviceModel GetComDevices(XElement xml)
        {
            ComDeviceModel output = new();

            foreach (var dev in xml.Elements("Device"))
            {
                switch (dev.Attribute("Type").Value)
                {
                    case "39":
                        output.SerialDevices = GetSerialDevices(dev);
                        break;
                    case "40":
                        output.MultiportSerialDevices = GetSerialDevices(dev);
                        break;
                    case "56":
                        output.LonInterface = dev.Elements("Property")
                            .Where(x => x.Attribute("Type").Value == "32")
                            .FirstOrDefault()
                            .Value;
                        break;
                    default:
                        break;
                }
            }

            return output;
        }

        private List<SerialDeviceModel> GetSerialDevices(XElement xml)
        {
            List<SerialDeviceModel> output = new();
            var nrSerPorts = xml.Elements("Property")
                .Where(x => x.Attribute("Type").Value == "83")
                .FirstOrDefault()
                .Value;

            if (nrSerPorts == "0")
                return output;

            foreach (var dev in xml.Elements("Device"))
            {
                SerialDeviceModel serDev = new();
                foreach (var prop in dev.Elements("Property"))
                {
                    
                    switch (prop.Attribute("Type").Value)
                    {
                        case "84":
                            serDev.PortName = prop.Value;
                            break;
                        case "85":
                            serDev.Device = prop.Value;
                            break;
                        default:
                            break;
                    } 
                }
                output.Add(serDev);
            }

            return output;
        }


        private TankManagementModel GetTankManagement(XElement xml)
        {
            TankManagementModel output = new();
            xml = xml.Elements("Device")
                .Where(x => x.Attribute("Type").Value == "12")
                .FirstOrDefault();

            output.TankGauge = xml.GetPropType34();

            foreach (var item in xml.Elements("Device").Where(x => x.Attribute("Type").Value == "13"))
            {
                TankGroupModel group = new();

                group.Number = int.Parse(item.Attribute("Number").Value);

                foreach (var tg in item.Elements("Property"))
                {
                    switch (tg.Attribute("Type").Value)
                    {
                        case "11":
                            group.ProductNumber = int.Parse(tg.Value);
                            break;
                        case "12":
                            group.ProductName = tg.Value;
                            break;
                        default:
                            break;
                    }
                }

                output.TankGroups.Add(group);
            }

            return output;
        }
    }
}
