using System;
using System.Collections.Generic;
using System.Text;

namespace TsgSystemsToolkit.DataManager.Models
{
    public class POSDbModel
    {
        public int Id { get; set; }
        public string StationId { get; set; }
        public int Number { get; set; }
        public string Type { get; set; }
        public string OperatingSystem { get; set; }
        public string HardwareType { get; set; }
        public string SoftwareVersion { get; set; }
        public string PrimaryIP { get; set; }
        public string ReceiptPrinter { get; set; }
        public string CustomerDisplay { get; set; }
        public string BarcodeScanner { get; set; }
        public string LevelGauge { get; set; }
        public string TouchScreenType { get; set; }
        public string UPS { get; set; }
        public int NumSerialPorts { get; set; }
    }
}
