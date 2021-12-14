using System;
using System.Collections.Generic;
using System.Linq;

namespace FuelPOS.StatDevParser.Models
{
    public class PosDetailModel
    {
        private const string NODEVICE = "Not found";
        public string Type { get; set; } = NODEVICE;
        public string OperatingSystem { get; set; } = NODEVICE;
        public string BuildDisk { get; set; } = NODEVICE;
        public int Number { get; set; }
        public string HardwareType { get; set; } = NODEVICE;
        public string SoftwareVersion { get; set; } = NODEVICE;
        public string PrimaryIP { get; set; } = NODEVICE;
        public string GatewayIP { get; set; } = NODEVICE;
        public string ReceiptPrinter { get; set; } = NODEVICE;
        public string PinPad { get; set; } = NODEVICE;
        public string A4Printer { get; set; } = NODEVICE;
        public string MagCardReader { get; set; } = NODEVICE;
        public string PaymentTerminal { get; set; } = NODEVICE;
        public string LoyaltyTerminal { get; set; } = NODEVICE;
        public string CustomerDisplay { get; set; } = NODEVICE;
        public string BarcodeScanner { get; set; } = NODEVICE;
        public ComDeviceModel ComDevices { get; set; }
        public List<DispensingModel> Dispensing { get; set; } = new();
        public string UPS { get; set; } = NODEVICE;
        public string TouchScreenType { get; set; } = NODEVICE;

        public List<string> DispenserCommTypes 
        { 
            get 
            {
                return Dispensing.Select(x => x.Protocol)
                    .Distinct()
                    .ToList();
            } 
        }
    }
}
