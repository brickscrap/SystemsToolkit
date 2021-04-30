using System.Collections.Generic;

namespace FuelPOS.StatDevParser.Models
{
    public class StatdevModel
    {
        public StationInfoModel StationInfo { get; set; }
        public List<PCInfoModel> POS { get; set; }
        public List<PumpModel> Dispensers { get; set; }
    }
}
