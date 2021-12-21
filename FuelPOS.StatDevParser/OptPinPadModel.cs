using FuelPOS.StatDevParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelPOS.StatDevParser
{
    public class OptPinPadModel : PinPadModel
    {
        public bool OaseKey { get; set; }
        public bool CtacKey { get; set; }
    }
}
