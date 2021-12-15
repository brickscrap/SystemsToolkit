using System.Collections.Generic;

namespace FuelPOS.StatDevParser.Models
{
    public class TankManagementModel
    {
        public string TankGauge { get; set; }
        public List<TankGroupModel> TankGroups { get; set; } = new();
    }
}
