using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelPOS.StatDevParser.Models
{
    public class TankManagementModel
    {
        public string TankGauge { get; set; }
        public List<TankGroupModel> TankGroups { get; set; } = new();
    }
}
