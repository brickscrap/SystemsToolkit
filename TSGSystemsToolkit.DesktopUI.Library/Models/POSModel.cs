using System;
using System.Collections.Generic;
using System.Text;

namespace TSGSystemsToolkit.DesktopUI.Library.Models
{
    public class POSModel
    {
        public string Type { get; set; }
        public string OperatingSystem { get; set; }
        public int Number { get; set; }
        public string HardwareType { get; set; }
        public string SoftwareVersion { get; set; }
        public string PrimaryIP { get; set; }
        public string ReceiptPrinter { get; set; }
        public string CustomerDisplay { get; set; }
        public string BarcodeScanner { get; set; }
        public string LevelGauge { get; set; }
        public string TouchScreen { get; set; }
        public int NumSerialPorts { get; set; }
        public List<SerialDeviceModel> SerialDevices { get; set; }
        public string UPS { get; set; }
    }
}
