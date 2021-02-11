using System;
using System.Collections.Generic;
using System.Text;

namespace FuelPOSToolkitDataManager.Library.Models
{
    public class SerialDeviceDbModel
    {
        public int Id { get; set; }
        public int POSHardwareId { get; set; }
        public string PortNumber { get; set; }
        public string Device { get; set; }
    }
}
