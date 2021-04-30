using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser.Models.TRX
{
    public class TrxDiscountModel
    {
        public Discount Type { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }

        public enum Discount : ushort
        {
            NoDiscount = 0,
            ManualDiscountIndividual = 1,
            PromoDiscount = 2,
            OfflineLoyalty = 3,
            Voucher = 4,
            BonusBarcode = 5,
            ConnectedPayTerm = 6,
            TotalTrx = 7,
            ManualDiscountTotal = 8,
            AutoDiscount = 9,
            OnlineLoyaltyArticlePartial = 11,
            OnlineLoyaltyArticleFull = 12,
            OnlineLoyaltyFuelPartial = 13,
            LoyaltyGiftHTEC = 15,
            TwoTieredPricing = 16,
            LoyaltyGiftOASE = 22,
            BNA = 23,
            PresetPercentage = 24,
            MobilePaymentHost = 25
        }
    }
}
