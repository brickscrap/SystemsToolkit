using System;
using System.Collections.Generic;

namespace POSFileParser.Models.TRX
{
    public class TrxMopModel : MappableBase<TrxMopModel>
    {
        private protected override IDictionary<string, Func<TrxMopModel, string, TrxMopModel>> Mappings
        {
            get { return _mappings; }
        }
        public MoP? MethodOfPayment { get; set; }
        public int AdditionalInfoX { get; set; }
        public int AdditionalInfoY { get; set; }
        public string CardPAN { get; set; }
        public RoP? ReasonOfPayment { get; set; }
        public int PaymentMode { get; set; }
        public int PaymentSubtype { get; set; }
        public double PaymentAmount { get; set; }

        private IDictionary<string, Func<TrxMopModel, string, TrxMopModel>> _mappings = new Dictionary<string, Func<TrxMopModel, string, TrxMopModel>>
        {
            { "MOP", (model, value) => { model.MethodOfPayment = (MoP)Enum.Parse(typeof(MoP), value); return model; } },
            { "CPPAN", (model, value) => { model.CardPAN = value; return model; } },
            { "MOP_ADDX", (model, value) => { model.AdditionalInfoX = int.Parse(value); return model; } },
            { "MOP_ADDY", (model, value) => { model.AdditionalInfoY = int.Parse(value); return model; } },
            { "ROP", (model, value) => { model.ReasonOfPayment = (RoP)Enum.Parse(typeof(RoP), value); return model; } },
            { "PM", (model, value) => { model.PaymentMode = int.Parse(value); return model; } },
            { "PSUB", (model, value) => { model.PaymentSubtype = int.Parse(value); return model; } },
            { "PA", (model, value) => { model.PaymentAmount = double.Parse(value); return model; } },
        };
    }

    public enum MoP : ushort
    {
        Cash = 1,
        ExtraPaymentMode = 2,
        LitreCoupon = 3,
        Card = 4,
        DebitCard = 5,
        PaymentTerminal = 6,
        Banksys = 7,
        NotPaid = 8,
        Unknown = 9,
        LocalAccount = 10,
        BNA = 11,
        PumpTest = 12,
        ExtPOS = 13,
        OfflinePaymentVoucher = 14,
        OnlinePaymentVoucher = 15,
        OnlineLoyaltyPoints = 16,
        PaymentTerminalVoucher = 17,
        ExternalEPR = 18,
        OfflineBNACredNote = 19,
        PrepaymentDifference = 20,
        RoundingOffCashDiff = 21,
        RoundingOffCardExtPayTermDiff = 22,
        RoundOffCardViaHost = 25,
        ExtPayTerminal = 40,
        MPR = 41,
        OutdoorAuthCode = 42,
        MPS = 43
    }
    public enum RoP : ushort
    {
        PurchaseFuelOrShop = 1,
        ChangeMoney = 2,
        BNARefund = 4,
        SafeDrop = 5,
        ReceiptPayIn = 6,
        ExpensePayOut = 7,
        LocalAccPayment = 8,
        SettleDelayedPayment = 9,
        PayInvoice = 10,
        PointsForMoney = 13,
        LoyaltyGift = 14,
        LoyaltyPoints = 15,
        BNACreditNote = 16,
        CashMacDumpCoins = 17,
        CashMacRefill = 18,
        CashMacExchange = 19,
        OfflineVoucherRefund = 20,
        SettleDriveoff = 21,
        RechargeCardOASE = 22
    }
}
