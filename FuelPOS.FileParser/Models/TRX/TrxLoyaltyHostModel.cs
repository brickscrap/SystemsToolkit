using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser.Models.TRX
{
    public class TrxLoyaltyHostModel
    {
        public string OfferID { get; set; }
        public double DirectDiscount { get; set; }
        public string DiscountName { get; set; }
        public bool CalculateVATOnGross { get; set; }
        public double PointsUsed { get; set; }
    }
}
