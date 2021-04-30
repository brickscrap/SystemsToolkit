using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser.Models.TRX
{
    public class TrxFuelBonusModel
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string SecondaryBarcode { get; set; }
        public string FuelUsedBarcode { get; set; }
        public FuelBonus BonusType { get; set; }

        public enum FuelBonus : ushort
        {
            Generic = 1,
            InternalCoupon = 2,
            ExternalCoupon = 3,
            PromoCode = 4
        }
    }
}
