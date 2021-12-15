namespace POSFileParser.Models.TRX
{
    public class TrxChargepointModel
    {
        public int ChargepointNumber { get; set; }
        public double EnergyPrice { get; set; }
        public double NonWMDiscountPrice { get; set; }
        public double StartFee { get; set; }
        public int UnitType { get; set; }
        public double Quantity { get; set; }
        public double Amount { get; set; }
        public double NonWMDiscountAmount { get; set; }

    }
}
