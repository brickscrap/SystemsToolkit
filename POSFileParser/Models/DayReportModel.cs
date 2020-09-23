using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser.Models
{
    public class DayReportModel
    {
        public List<StatusModel> Status { get; set; }
        public List<HeaderReceiptModel> HeaderReceipt { get; set; }
        public List<MerchantInfoModel> MerchantInfo { get; set; }
        public List<FuelInfoModel> StartFuelInfo { get; set; }
        public List<FuelInfoModel> FuelInfo { get; set; }
        public List<ArticleSoldInfoModel> ArticleSoldInfo { get; set; }
        public List<CurrencyInfoModel> CurrencyInfo { get; set; }
        public List<LionLoyaltyModel> LionLoyalty { get; set; }
    }
}
