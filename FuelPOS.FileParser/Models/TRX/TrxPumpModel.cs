using System;

namespace POSFileParser.Models.TRX
{
    public class TrxPumpModel
    {
        public int PumpNumber { get; set; }
        public string PumpMode { get; set; }
        public int NozzleNumber { get; set; }
        public DateTime NozzleReturnTime { get; set; }
        public DateTime FillingStartTime { get; set; }
        public string TagID { get; set; }
        public double FuelBasePrice { get; set; }
        public double ProgrammedPriceDifference { get; set; }
        public AltFuelPriceModel AlternatePrice { get; set; }
        public double CalculatorAmount { get; set; }
        public double CalculatorQuantity { get; set; }
        public double NozzleQuantityChangeable { get; set; }
    }
}
