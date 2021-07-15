using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelPOS.TankTableTools.Models
{
    public class CsvOutputModel
    {
        public string TankNumber { get; set; }
        public string Grade { get; set; }
        public string Theoretical { get; set; }
        public string Operational { get; set; }
        public string Height { get; set; }
        public string StockOut { get; set; }
    }
}
