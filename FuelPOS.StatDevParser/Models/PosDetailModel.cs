using System.Collections.Generic;
using System.Linq;

namespace FuelPOS.StatDevParser.Models
{
    public class PosDetailModel
    {
        public string Type { get; set; } = Constants.NOT_FOUND;
        public string OperatingSystem { get; set; } = Constants.NOT_FOUND;
        public string BuildDisk { get; set; } = Constants.NOT_FOUND;
        public int Number { get; set; } = 0;
        public string HardwareType { get; set; } = Constants.NOT_FOUND;
        public string SoftwareVersion { get; set; } = Constants.NOT_FOUND;
        public string PrimaryIP { get; set; } = Constants.NOT_FOUND;
        public string GatewayIP { get; set; } = Constants.NOT_FOUND;
        public string ReceiptPrinter { get; set; } = Constants.NOT_FOUND;
        public PinPadModel PinPad { get; set; } = new();
        public string A4Printer { get; set; } = Constants.NOT_FOUND;
        public string MagCardReader { get; set; } = Constants.NOT_FOUND;
        public string PaymentTerminal { get; set; } = Constants.NOT_FOUND;
        public string LoyaltyTerminal { get; set; } = Constants.NOT_FOUND;
        public string CustomerDisplay { get; set; } = Constants.NOT_FOUND;
        public string BarcodeScanner { get; set; } = Constants.NOT_FOUND;
        public List<OptModel> OutdoorTerminals { get; set; } = new();
        public ComDeviceModel ComDevices { get; set; } = new();
        public List<DispensingModel> Dispensing { get; set; } = new();
        public string UPS { get; set; } = Constants.NOT_FOUND;
        public string TouchScreenType { get; set; } = Constants.NOT_FOUND;

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
