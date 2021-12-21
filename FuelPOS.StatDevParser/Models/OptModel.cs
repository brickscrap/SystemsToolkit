using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelPOS.StatDevParser.Models
{
    public class OptModel
    {
        public string Number { get; set; } = Constants.NOT_FOUND;
        public string Type { get; set; } = Constants.NOT_FOUND;
        public string SoftwareVersion { get; set; } = Constants.NOT_FOUND;
        public string HardwareType { get; set; } = Constants.NOT_FOUND;
        public OptPinPadModel PinPad { get; set; } = new();
    }
}
