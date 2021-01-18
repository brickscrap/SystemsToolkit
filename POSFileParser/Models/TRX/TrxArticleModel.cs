using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser.Models.TRX
{
    public class TrxArticleModel : MappableBase<TrxArticleModel>
    {
        #region Public Properties
        private protected override IDictionary<string, Func<TrxArticleModel, string, TrxArticleModel>> Mappings
        { 
            get { return _mappings; } 
        }
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
        public string PLU { get; set; }


        // TODO : COMP_LEVELx,y,r page 977

        #endregion

        private IDictionary<string, Func<TrxArticleModel, string, TrxArticleModel>> _mappings = new Dictionary<string, Func<TrxArticleModel, string, TrxArticleModel>>
        {
            { "PRI", (model, value) => { model.Price = double.Parse(value); return model; } },
            { "UPRI", (model, value) => { model.UnitPrice = double.Parse(value); return model; } },
            { "QTY", (model, value) => { model.Quantity = double.Parse(value); return model; } },
            { "QTYUNIT", (model, value) => { model.QuantityUnits = (Unit)Enum.Parse(typeof(Unit), value); return model; } },
            { "GAMT", (model, value) => { model.GrossAmount = double.Parse(value); return model; } },
            { "AMT", (model, value) => { model.NetAmount = double.Parse(value); return model; } },
            { "PLU", (model, value) => { model.PLU = value; return model; } },
            { "NAM", (model, value) => { model.Name = value; return model; } },
            { "GRP", (model, value) => { model.GroupCode = value; return model; } },
            { "CRD", (model, value) => { model.CardCode = value; return model; } },
            { "ART_MERCH_ID", (model, value) => { model.MerchantID = value; return model; } },
            { "PRC", (model, value) => { model.VATPercentage = double.Parse(value); return model; } },
            { "VAMT", (model, value) => { model.VATAmount = double.Parse(value); return model; } },
            { "EXC", (model, value) => { model.AmountVATExcluded = double.Parse(value); return model; } },
            { "PMP", (model, value) => { model.PumpDetails = new TrxPumpModel(); model.PumpDetails.PumpNumber = int.Parse(value); return model; } },
            { "PUMP_MODE", (model, value) => { model.PumpDetails.PumpMode = value; return model; } },
            { "NOZZLE", (model, value) => { model.PumpDetails.NozzleNumber = int.Parse(value); return model; } },
            { "NDATI", (model, value) => { model.PumpDetails.NozzleReturnTime = value.ParseFuelPOSDate(); return model; } },
            { "START_DATE", (model, value) => { model.PumpDetails.FillingStartTime = value.ParseFuelPOSDate(); return model; } },
            { "BASEPRI", (model, value) => { model.PumpDetails.FuelBasePrice = double.Parse(value); return model; } },
            { "EXTREF", (model, value) => { model.ExternalReference = value; return model; } },
            { "NQUA_CHANGEABLE", (model, value) => { model.PumpDetails.NozzleQuantityChangeable = double.Parse(value); return model; } },
        };

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
