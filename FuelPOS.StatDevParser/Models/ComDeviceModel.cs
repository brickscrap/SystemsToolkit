using System.Collections.Generic;
using System.Linq;

namespace FuelPOS.StatDevParser.Models
{
    public class ComDeviceModel
    {
        public int NumberSerialPortsInUse
        {
            get
            {
                return SerialDevices
                    .Where(x => x.Device.Trim() != "Unknown")
                    .Count();
            }
        }
        public List<SerialDeviceModel> SerialDevices { get; set; } = new();

        public int NumberPciMultiPortsInUse
        {
            get
            {
                return MultiportSerialDevices
                    .Where(x => x.Device.Trim() != "Unknown")
                    .Count();
            }
        }
        public List<SerialDeviceModel> MultiportSerialDevices { get; set; } = new();
        public string LonInterface { get; set; } = Constants.NOT_FOUND;
    }
}
