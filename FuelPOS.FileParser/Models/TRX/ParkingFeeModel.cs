using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser.Models.TRX
{
    public class ParkingFeeModel
    {
        public double BaseFee { get; set; }
        public DateTime ParkingStartTime { get; set; }
        public DateTime ParkingEndTime { get; set; }
        public double ParkingFeeQuantity { get; set; }
        public double ParkingFeeAmount { get; set; }
        public double FreeParkingTime { get; set; }
        
    }
}
