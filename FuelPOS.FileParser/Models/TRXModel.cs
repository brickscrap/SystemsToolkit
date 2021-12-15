using POSFileParser.Enums;
using POSFileParser.Models.TRX;
using SharpConfig;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POSFileParser.Models
{
    public class TRXModel : MappableBase<TRXModel>, IFileSection
    {
        #region Public Properties

        private protected override IDictionary<string, Func<TRXModel, string, TRXModel>> Mappings
        {
            get { return _mappings; }
        }
        public string IDKey { get; set; }
        /// <summary>Defines the type of transaction.</summary>
        public TrxType? Type { get; set; }
        /// <summary>Indicates a special transaction performed by the cashier.</summary>
        public SpecType? SpecialType { get; set; }
        /// <summary>Date and time of the transaction.</summary>
        public DateTime? TrxDateTime { get; set; }
        /// <summary>The station number.</summary>
        public string StationNumber { get; set; }
        /// <summary>The shift number. This field only exists for transactions of devices 11 to 18</summary>
        public int ShiftNumber { get; set; }
        /// <summary>This field provides the accounting day report number of transaction.</summary>
        public int AccRepNumber { get; set; }
        /// <summary>This field provides by means of a code an indication why a zero transaction was recorded.</summary>
        public ZeroTrxType? ZeroTrxCode { get; set; }
        /// <summary>Day report number of the transaction.</summary>
        public int PeriodNumber { get; set; }
        /// <summary>
        /// Attendant report number of transaction x.
        /// This field only exists if the attendant report is applied in the station.
        /// </summary>
        public int AttendantRepNumber { get; set; }
        /// <summary>Identification of the cashier who processed the transaction.</summary>
        public string OperatorID { get; set; }
        /// <summary>
        /// Reference number of transaction x, as it is printed on the customer receipt. It is a numbering 
        /// that restarts after a day report closure.
        /// </summary>
        public string RefNumber { get; set; }
        /// <summary>This field contains the reference number of the transaction to which it is linked.</summary>
        public string LinkedRefNumber { get; set; }
        /// <summary>
        /// Sequence number of transaction x. This numbering continues after a day report closure, contrary 
        /// to the transaction reference number
        /// </summary>
        public string SequenceNumber { get; set; }
        /// <summary>
        /// When the cashier has the possibility to enter a reference when closing the shift, then 
        /// the entered value is provided in this field.
        /// </summary>
        public string ShiftClosureRef { get; set; }
        /// <summary>
        /// This field indicates whether or not the Fuel POS was connected to the CIS on the moment the report was closed.
        /// </summary>
        public bool POSDisconnected { get; set; }
        /// <summary>This field gives the code of the current local currency of the Fuel POS.</summary>
        public CurrencyType? LocalCurrency { get; set; }
        /// <summary>
        /// This field can only be available in one particular situation.
        /// A back office can send an article mutation file to modify the fuel prices.In this file, 
        /// the back office can put a reference field of maximum 4 digits (one reference for all fuel types). 
        /// If this reference is sent, a day closure will start automatically when the new fuel prices are 
        /// activated. Only if it had been sent, this field gives for every transaction the reference 
        /// number that is currently active.
        /// </summary>
        public int FuelPriceChangeRef { get; set; }
        /// <summary>
        /// If a loyalty card has been read for this transaction, then this field contains the 
        /// card type as defined by Tokheim.
        /// </summary>
        public int LoyaltyType { get; set; }
        /// <summary>
        /// This field contains the PAN of the loyalty card that has been used for the transaction.
        /// </summary>
        public string LoyaltyPAN { get; set; }
        /// <summary>The Airmiles number of the transaction.</summary>
        public string Airmiles { get; set; }
        /// <summary>
        /// Number of on-line loyalty points, saved by the customer. In this situation the loyalty 
        /// points are provided for the entire transaction and not for each individual sales item. 
        /// This field is applied for each type of on-line loyalty host.
        /// </summary>
        public double LoyaltyPointsOnline { get; set; }
        /// <summary>
        /// Number of off-line loyalty points, saved by the customer. The points are only 
        /// provided in this way if they are restricted to fuels and if they are calculated 
        /// on the total amount of the fuels in the transaction.
        /// </summary>
        public double LoyaltyPointsOfflineFuelAmt { get; set; }
        /// <summary>
        /// Number of off-line loyalty points, saved by the customer. The points are only 
        /// provided in this way if they are restricted to fuels and if they are calculated 
        /// on the total volume of the fuels in the transaction.
        /// </summary>
        public double LoyaltyPointsOfflineFuelVol { get; set; }
        /// <summary>Number of on-line bonus points, saved by the customer.</summary>
        public double BonusPointsOnline { get; set; }
        /// <summary>
        /// When a loyalty card has been read during the transaction, then the operator can 
        /// scan the bar code of one or more bonus coupons. The bar code number of the bonus 
        /// coupon that has been read is provided in this field. Item1 represents the
        /// sequence number of the coupon. Item2 represents the barcode.
        /// </summary>
        public List<BonusCouponModel> BonusCoupon { get; set; } = new List<BonusCouponModel>();
        /// <summary>Details of any gifts attached to the transaction.</summary>
        public List<GiftModel> Gifts { get; set; } = new List<GiftModel>();
        /// <summary>
        /// A customer can redeem on-line loyalty points and have the according amount
        /// deposited on his bank account or paid back in cash. This field gives the 
        /// number of redeemed points.
        /// </summary>
        public double RedeemedLoyaltyPoints { get; set; }
        /// <summary>
        /// A customer can redeem on-line loyalty points and have the according amount
        /// deposited on his bank account or paid back in cash. This field gives the amount 
        /// that will be put on the bank account or that will be paid in cash.
        /// </summary>
        public double RedeemedLoyaltyAmount { get; set; }
        /// <summary>
        /// This field provides an indicative loyalty discount. This means that a discount is 
        /// awarded, but this discount is not yet applied in the transaction.
        /// It is printed on the customer receipt and will be collected by a host for 
        /// further processing.
        /// </summary>
        public double IndicativeLoyaltyDiscount { get; set; }
        /// <summary>
        /// A customer can order in the station a present (gift) from a loyalty catalogue.
        /// He can collect this ordered present at a later moment. This field provides the
        /// order number on the moment the present is collected by the customer.
        ///This field only applies to on-line loyalty via HTEC.
        /// </summary>
        public string LoyaltyOrderNumber { get; set; }
        /// <summary>Details of the local account customer.</summary>
        public CustomerModel Customer { get; set; }
        /// <summary>Contains the number of PIN attempts performed. </summary>
        public int PINAttempts { get; set; }
        /// <summary>
        /// In case of an on-line card transaction, this field indicates which host is 
        /// contacted by Fuel POS to process the transaction.
        /// </summary>
        public HostDetailModel HostDetail { get; set; }
        /// <summary>
        /// This field is only available for card transactions that are authorized and/or 
        /// booked via an on-line host. It contains the response code as provided by the host.
        /// </summary>
        public string OnlineResponseCode { get; set; }
        /// <summary>
        /// This field is used in case the transaction is paid by means of a card.
        /// The field links the transaction to a reference, given by the card issuer 
        /// authorisation centre.
        /// </summary>
        public string AuthCode { get; set; }
        /// <summary>
        /// This field is available for card transactions that are authorised and/or booked 
        /// via an on-line host. It contains the STAN as used in the communication with the host. 
        /// </summary>
        public string STAN { get; set; }
        /// <summary>Indicates whether it is the customers own car or a replacement car.</summary>
        public ReplacementCarType? ReplacementCar { get; set; }
        /// <summary>Mileage of the car as entered by the customer.</summary>
        public int OdometerReading { get; set; }
        /// <summary>Driver ID as entered by the customer.</summary>
        public int DriverID { get; set; }
        /// <summary>Licence plate as entered by the customer.</summary>
        public string LicencePlate { get; set; }
        /// <summary>Additional data entered by the customer.</summary>
        public string AdditionalData { get; set; }
        /// <summary>Vehicle identification entered by customer or read from card.</summary>
        public string VehicleID { get; set; }
        /// <summary>Additional information provided by the cashier.</summary>
        public string TrxInfo { get; set; }
        /// <summary>
        /// This field provides the PAN of an ID card if provided/used.
        /// </summary>
        public string IDCardNumber { get; set; }
        /// <summary>
        /// Only used for transaction type 6. Unique terminal ID of connected banksys
        /// indoor payment terminal.
        /// </summary>
        public string TerminalID { get; set; }
        /// <summary>
        /// 6 digit credit note provided by BNA or EV Charger.
        /// </summary>
        public int BNAEVCreditNote { get; set; }
        /// <summary>
        /// Total net amount of all sales items in the transaction.
        /// </summary>
        public double SalesItemNetTotal { get; set; }
        /// <summary>
        /// Indicates how some specific transactions are originated.
        /// </summary>
        public TrxOriginator? Originator { get; set; }
        /// <summary>Details of a credit note deleted or paid back to the customer.</summary>
        public BNACreditNoteModel CreditNoteDetail { get; set; }
        /// <summary>
        /// This field provides the name of the Jpeg file linked to this transaction (when
        /// using an electronic signature pad).
        /// </summary>
        public string ESigFileName { get; set; }
        /// <summary>
        /// The number of a discount voucher for the transaction.
        /// </summary>
        public int DiscountVoucherNumber { get; set; }
        /// <summary>
        /// The number of discount vouchers used in this transaction.
        /// </summary>
        public int DiscountVouchersAccepted { get; set; }
        public PaymentVoucherModel PaymentVoucherDeleted { get; set; }

        // TODO: Add special payment methods, manual page 943-946

        /// <summary>
        /// Contains a list of different merchant receipts, if FuelPOS is configured with
        /// multiple different merchants.
        /// </summary>
        public List<MerchantReceiptModel> MerchantReceipts { get; set; } = new List<MerchantReceiptModel>();
        /// <summary>
        /// Details the chargepoint number per transaction item.
        /// </summary>
        public TrxChargepointModel ChargepointDetail { get; set; }
        public List<TrxArticleModel> Articles { get; set; } = new List<TrxArticleModel>();
        public List<TrxMopModel> MethodsOfPayment { get; set; } = new List<TrxMopModel>();
        public double SalesTotal { get; set; }

        #endregion

        private IDictionary<string, Func<TRXModel, string, TRXModel>> _mappings = new Dictionary<string, Func<TRXModel, string, TRXModel>>
        {
            { "TYP", (model, value) => { model.Type = (TrxType)Enum.Parse(typeof(TrxType), value); return model; } },
            { "SPEC_TYP", (model, value) => { model.SpecialType = (SpecType)Enum.Parse(typeof(SpecType), value); return model; } },
            { "DATI", (model, value) => { model.TrxDateTime = value.ParseFuelPOSDate(); return model; } },
            { "STATION_NR", (model, value) => { model.StationNumber = value; return model; } },
            { "SHF", (model, value) => { model.ShiftNumber = int.Parse(value); return model; } },
            { "ACC_REP_NR", (model, value) => { model.AccRepNumber = int.Parse(value); return model;  } },
            { "ZERO_TRX_CODE", (model, value) => { model.ZeroTrxCode = (ZeroTrxType)Enum.Parse(typeof(ZeroTrxType), value); return model; } },
            { "PER", (model, value) => { model.PeriodNumber = int.Parse(value); return model; } },
            { "ATTENDANT_REPORT_NR", (model, value) => { model.AttendantRepNumber = int.Parse(value); return model; } },
            { "OPID", (model, value) => { model.OperatorID = value; return model; } },
            { "REF", (model, value) => { model.RefNumber = value; return model; } },
            { "LREF", (model, value) => { model.LinkedRefNumber = value; return model; } },
            { "SEQ", (model, value) => { model.SequenceNumber = value; return model; } },
            { "SHIFTCLOSEREF", (model, value) => { model.ShiftClosureRef = value; return model; } },
            { "POSDISCONNECTED", (model, value) => { model.POSDisconnected = value.StringToBool(); return model; } },
            { "LCUR", (model, value) => { model.LocalCurrency = (CurrencyType)Enum.Parse(typeof(CurrencyType), value); return model; } },
            { "FPRICESEQ", (model, value) => { model.FuelPriceChangeRef = int.Parse(value); return model; } },
            { "LTYP", (model, value) => { model.LoyaltyType = int.Parse(value); return model; } },
            { "LPAN", (model, value) => { model.LoyaltyPAN = value; return model; } },
            { "AIR", (model, value) => { model.Airmiles = value; return model; } },
            { "LPNT", (model, value) => { model.LoyaltyPointsOnline = double.Parse(value); return model; } },
            { "LAPNT", (model, value) => { model.LoyaltyPointsOfflineFuelAmt = double.Parse(value); return model; } },
            { "LVPNT", (model, value) => { model.LoyaltyPointsOfflineFuelVol = double.Parse(value); return model; } },
            { "BPNT", (model, value) => { model.BonusPointsOnline = double.Parse(value); return model; } },
            { "SLSTOT", (model, value) => { model.SalesItemNetTotal = double.Parse(value); return model; } }
        };

        public IFileSection Create(IGrouping<string, Setting> groupedData)
        {
            var subGroups = groupedData.GroupBy(r => r.Name.SplitKey().Length > 2 ? r.Name.SplitKey()[2] : null);

            if (subGroups.Count() > 1)
            {
                foreach (var item in subGroups)
                {
                    if (item.Key == null)
                    {
                        foreach (var subItem in item)
                        {
                            MapValue(subItem, this);
                        }
                    }
                    else
                    {
                        if (item.First().Name.StartsWith("MERCH_ID"))
                        {
                            var merchant = item.Where(i => item.Any(s => i.Name.StartsWith("MERCH_")));
                            MerchantReceiptModel merch = new MerchantReceiptModel();
                            foreach (var line in merchant)
                            {
                                merch.MapValue(line, merch);
                            }
                            MerchantReceipts.Add(merch);
                        }
                        if (item.Any(x => x.Name.Contains("MOP")))
                        {
                            TrxMopModel methodOfPayment = new TrxMopModel();
                            foreach (var subItem in item)
                            {
                                methodOfPayment.MapValue(subItem, methodOfPayment);
                            }

                            MethodsOfPayment.Add(methodOfPayment);
                        }
                        TrxArticleModel article = new TrxArticleModel();

                        foreach (var subItem in item)
                        {
                            article.MapValue(subItem, article);
                        }

                        Articles.Add(article);
                    }
                }
            }
            else
            {
                foreach (var item in groupedData)
                {
                    MapValue(item, this);
                }
            }

            return this;
        }
        public bool ShouldSerializeMethodsOfPayment()
        {
            return MethodsOfPayment.Count > 0;
        }

        public bool ShouldSerializeArticles()
        {
            return Articles.Count > 0;
        }

        public bool ShouldSerializeMerchantReceipts()
        {
            return MerchantReceipts.Count > 0;
        }

        public bool ShouldSerializeBonusCoupon()
        {
            return BonusCoupon.Count > 0;
        }

        public bool ShouldSerializeGifts()
        {
            return Gifts.Count > 0;
        }
    }



    /// <summary>
    /// Defines possible transaction types
    /// </summary>
    public enum TrxType : ushort
    {
        /// <summary>The transaction is an open shift message.</summary>
        OpenShift = 1,
        /// <summary>A normal transaction.</summary>
        NormalTrx = 2,
        /// <summary>The transaction is a close shift message.</summary>
        CloseShift = 3,
        /// <summary>The transaction is a message stating that an accounting day has been closed.</summary>
        AccDayClose = 4,
        /// <summary>The transaction is the cancellation of a previously terminated transaction.</summary>
        CancelPrevTrx = 5,
        /// <summary>
        /// The transaction is a message that only exists if an indoor payment terminal has been 
        /// connected to Fuel POS. It indicates that a day report has been closed in Fuel POS. This day 
        /// closure forces for some connected payment terminals a closure on that terminal as well. 
        /// If the connected payment terminal is closed, the closure information of that terminal will be given.
        /// </summary>
        DayCloseWithIPT = 6,
        /// <summary>
        /// If a customer comes back into the shop after finishing a prepaid filling or EV charge, 
        /// either to ask a VAT receipt or to ask for change if the prepaid amount was not reached, then 
        /// the original transaction is cancelled.
        /// </summary>
        PrepaidTrxCancel = 7,
        /// <summary>
        /// A customer redeemed on-line loyalty points and requested to deposit the corresponding 
        /// amount on his bank account, so he doesn’t get the money in cash. This transaction is started 
        /// on Fuel POS but has no effect on the reports of the station. This applies only to on-line 
        /// loyalty via a Xenta terminal.
        /// </summary>
        XentaLoyalty = 8,
        /// <summary>
        /// The customer ordered in the station a present (gift) from a loyalty catalogue. At the moment 
        /// of ordering, the on-line loyalty points are redeemed. If the customer needs to pay an additional
        /// amount, then this is only done when he collects the present. The collection, which takes place 
        /// later, is a normal transaction of type 2. This applies only to on-line loyalty via HTEC.
        /// </summary>
        PaidWithLoyalty = 9,
        /// <summary>The transaction is a message stating that a day has been opened.</summary>
        DayOpen = 10,
        /// <summary>The transaction is a message stating that a day has been closed.</summary>
        DayCloseNoIPT = 11,
        /// <summary>The transaction is a message stating that a new attendant report has been opened.</summary>
        AttendantRepOpened = 12,
        /// <summary>The transaction is a message stating that an attendant report has been closed.</summary>
        AttendantRepClosed = 13,
        /// <summary>
        /// An open off-line BNA credit note (issued by Fuel POS) has expired or has been deleted. 
        /// Deleting an off-line BNA credit note is possible via eMIS or by sending a BNA 
        /// mutation file.
        /// </summary>
        BNACreditNoteDelete = 14,
        /// <summary>
        /// The transaction is a message stating that the BNA has been opened to remove the money. 
        /// The BNA report which is created at that time by the Fuel POS is provided as a 
        /// special type of transaction.
        /// </summary>
        BNAOpened = 15,
        /// <summary>The transaction is a message stating that an accounting day has been opened.</summary>
        AccDayOpen = 16,
        /// <summary>
        /// A payment voucher has automatically been deleted because it has expired,
        /// or because it was used and the remaining balance has become zero. For these 2 
        /// situations, a parameter in Fuel POS defines whether or not the payment voucher 
        /// is automatically deleted. By default a payment voucher is never deleted automatically.
        /// </summary>
        VoucherDelete = 17,
        /// <summary>A local account card is blocked because of 3 wrong PIN attempts</summary>
        LocalAccCardBlockedWrongPIN = 18,
        /// <summary>A local account card, previously blocked because of 3 wrong PIN attempts, is unblocked again</summary>
        LocalAccCardUnblockedWrongPIN = 19,
        /// <summary>
        /// The transaction is a message indicating that the contents of the cash drawer has been entered
        /// in the programming screen (eMIS) for a shift that had been closed before. When this type of 
        /// transaction is in the file without any entered amounts, then this is because the cashier 
        /// indicated in Fuel POS that the cash drawer was empty on the moment of the shift closure.
        /// </summary>
        CashDrawerContentsEntered = 20,
        /// <summary>
        /// The transaction indicates a special cashier operation (deleting lines, voiding transactions, 
        /// opening the cash drawer manually…)
        /// </summary>
        SpecialCashierOperation = 21,
        /// <summary>
        /// This value is only possible in case the option to work with multiple cashiers on the same 
        /// shift has been enabled. It indicates that there was a user switch during a running shift.
        /// </summary>
        UserSwitchDuringShift = 22
    }
    /// <summary>
    /// This field is only available for transaction type 21 (special cashier operation). 
    /// It indicates what type of special action was performed by the cashier.
    /// </summary>
    public enum SpecType : ushort
    {
        /// <summary>The cashier removed one sales line containing a shop article from a transaction.</summary>
        RemovedOneArticleLine = 1,
        /// <summary>The cashier lowered the quantity of a shop article in a sales transaction.</summary>
        LoweredArticleQuantity = 2,
        /// <summary>The cashier removed one sales line containing a filling from a transaction.</summary>
        RemovedOneFillingLine = 3,
        /// <summary>The cashier removed one line containing a payment mode from a transaction.</summary>
        RemovedOnePayModeLine = 4,
        /// <summary>A shop article was removed by the cashier by voiding a complete transaction.</summary>
        ArticleRemovedTrxVoided = 5,
        /// <summary>A filling was removed by the cashier by voiding a complete transaction.</summary>
        FillingRemovedTrxVoided = 6,
        /// <summary>A payment mode was removed by the cashier by voiding a complete transaction.</summary>
        PaymentRemovedTrxVoided = 7,
        /// <summary>A card transaction was cancelled by the cashier.</summary>
        CardTrxCancelled = 8,
        /// <summary>A transaction via a connected payment terminal was cancelled by the cashier.</summary>
        PayTermTrxCancelled = 9,
        /// <summary>The cashier entered manually a card number instead of reading the card via the card reader.</summary>
        ManualCardEntry = 10,
        /// <summary>
        /// The cashier indicated for a card transaction that the customer did not need to enter his PIN 
        /// code and that the transaction was thus accepted with signature.
        /// </summary>
        CardSignedNoPIN = 11,
        /// <summary>The cashier processed an EMV transaction via the magnetic stripe of the card instead of the chip.</summary>
        MagCardNoChip = 12,
        /// <summary>The cashier has manually opened the cash drawer.</summary>
        OpenCashDrawer = 13,
        /// <summary>The cashier has modified the sales price of a shop article during a sales transaction.</summary>
        ModifiedArticlePrice = 14,
        /// <summary>
        /// When a shop article is added to the transaction to which a cashier message is linked (for instance 
        /// for an age verification), then the cashier can accept or refuse to sell the article. If it is 
        /// refused, then the article is automatically removed again from the transaction. 
        /// A special action with value 15 will be created.
        /// </summary>
        ArticleRefused = 15,
        /// <summary>
        /// When a shop article is added to the transaction of which the sale has been blocked by 
        /// the site manager, then a message is displayed which needs to be confirmed by the cashier. Upon
        /// confirmation the article is automatically removed again from the transaction. A special action
        /// with value 16 will be created.
        /// </summary>
        ArticleBlocked = 16,
        /// <summary>
        /// Some articles can only be sold as part of a promotion. If such an article is selected, 
        /// but the conditions for the promotion are not met, then the article is automatically removed again
        /// from the transaction when calculating the promotions. A special action with value 17 will be created.
        /// </summary>
        PromoOnly = 17,
        /// <summary>The cashier removed one sales line containing an EV charge from a transaction.</summary>
        EVChargeRemoved = 18,
        /// <summary>An EV charge was removed by the cashier by voiding a complete transaction.</summary>
        EVChargeRemovedTrxVoid = 19
    }
    /// <summary>
    /// This field provides by means of a code an indication why a zero transaction was recorded.
    /// </summary>
    public enum ZeroTrxType
    {
        /// <summary>
        /// The transaction is stopped by the customer, for instance by reinserting the 
        /// payment card on a moment the filling or recharge can normally already start.
        /// </summary>
        StoppedByCustomer = 3,
        /// <summary>An off-line payment card is refused</summary>
        OfflineRefused = 4,
        /// <summary>
        /// An on-line payment card is refused. If in this scenario a reversal is however 
        /// sent to the host, then no zero transaction will created and no info will be 
        /// available in the real time transaction file.
        /// </summary>
        OnlineRefused = 7,
        /// <summary>The transaction is automatically stopped, for instance because the 
        /// customer did not take a nozzle or connector.</summary>
        AutoStop = 10
    }
    public enum ReplacementCarType
    {
        OwnCar = 0,
        ReplacementCar = 1,
        NotAsked = 2
    }
    public enum TrxOriginator
    {
        LocalEmis = 1,
        AutoDelete = 9,
        Mutation = 10,
        RemoteEmis = 14
    }
}
