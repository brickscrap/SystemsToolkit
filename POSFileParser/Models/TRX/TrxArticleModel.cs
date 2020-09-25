using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser.Models.TRX
{
    public class TrxArticleModel : ICanParse
    {
        #region Public Properties
        public string IDKey { get; set; }
        public ArticleType Type { get; set; }
        public TrxPumpModel PumpDetails { get; set; }
        public TrxChargepointModel ChargepointDetails { get; set; }
        public TrxShopItemModel ShopItemDetails { get; set; }
        public double Price { get; set; }
        public double UnitPrice { get; set; }
        public double ProgrammedPrice { get; set; }
        public double ProgrammedUnitPrice { get; set; }
        public double DepositValue { get; set; }
        public double Quantity { get; set; }
        public Unit QuantityUnits { get; set; }
        public double DepositReturnedQuantity { get; set; }
        public double GrossAmount { get; set; }
        public double NetAmount { get; set; }
        public double DepositAmount { get; set; }
        public double GrossAmountNoDeposit { get; set; }
        public double NetAmountNoDeposit { get; set; }
        public ExternalType ExtType { get; set; }
        public string ExtCode { get; set; }
        public Voucher VoucherType { get; set; }
        public string Name { get; set; }
        public string GroupCode { get; set; }
        public string CardCode { get; set; }
        public string ExternalReference { get; set; }
        public string MerchantID { get; set; }
        public string PromotionID { get; set; }
        public TrxDiscountModel DiscountDetail { get; set; }
        public TrxLoyaltyHostModel LoyaltyHostDetail { get; set; }
        public double OfflineLoyaltyPoints { get; set; }
        public TrxFuelBonusModel FuelBonusDetail { get; set; }
        public double VATPercentage { get; set; }
        public double DepositVATPercentage { get; set; }
        public double VATAmount { get; set; }
        public double DepositVATAmount { get; set; }
        public double AmountVATExcluded { get; set; }
        public double DepositAmountVATExcluded { get; set; }
        // INCx,y
        public double PartialVATAmount { get; set; }
        // TODO : COMP_LEVELx,y,r page 977

        #endregion

        public void AddToItem(string[] headers, string value)
        {
            throw new NotImplementedException();
        }

        #region Enumerators
        public enum ArticleType : ushort
        {
            Fuel = 1,
            ShopGood = 2,
            EV = 3
        }
        public enum Unit : ushort
        {
            Piece = 1,
            Litre = 2,
            Kilogram = 3,
            kWh = 4
        }
        public enum ExternalType : ushort
        {
            FunParkTicket = 1,
            PhoneCard = 2,
            CarWash = 3,
            CigaretteVended = 4,
            GiftCard = 5,
            CreditReload = 6,
            PaymentVoucher = 7,
            ParkingTicket = 8,
            LotteryTicket = 12
        }
        public enum Voucher : ushort
        {
            FuelQuantity = 1,
            FuelAmount = 2,
            ShopGoods = 3
        }
        
        #endregion
    }
}
