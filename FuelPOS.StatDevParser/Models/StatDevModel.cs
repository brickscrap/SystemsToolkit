using System.Collections.Generic;

namespace FuelPOS.StatDevParser.Models
{
    public class StatdevModel
    {
        public StationInfoModel StationInfo { get; set; }
        public TankManagementModel TankManagement { get; set; }
        public List<PosDetailModel> POS { get; set; } = new();
    }
}
