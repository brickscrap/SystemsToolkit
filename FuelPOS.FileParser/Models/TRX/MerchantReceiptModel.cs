using System;
using System.Collections.Generic;

namespace POSFileParser.Models.TRX
{
    public class MerchantReceiptModel : MappableBase<MerchantReceiptModel>
    {
        private protected override IDictionary<string, Func<MerchantReceiptModel, string, MerchantReceiptModel>> Mappings
        {
            get { return _mappings; }
        }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public int VATCountryCode { get; set; }
        public string VATNumber { get; set; }
        public string SequenceNumber { get; set; }
        public string VATSequenceNumber { get; set; }
        public string SimplifiedInvoiceNumber { get; set; }

        private IDictionary<string, Func<MerchantReceiptModel, string, MerchantReceiptModel>> _mappings = new Dictionary<string, Func<MerchantReceiptModel, string, MerchantReceiptModel>>
        {
            { "MERCH_ID", (model, value) => { model.Identifier = value; return model; } },
            { "MERCH_NAME", (model, value) => { model.Name = value; return model; } },
            { "MERCH_VAT_ISO_COUNTRY", (model, value) => { model.VATCountryCode = int.Parse(value); return model; } },
            { "MERCH_RECEIPT_SEQ_NR", (model, value) => { model.SequenceNumber = value; return model; } },
            { "MERCH_VATNR", (model, value) => { model.VATNumber = value; return model; } },
            { "MERCH_VATSEQNR", (model, value) => { model.VATSequenceNumber = value; return model; } },
            { "MERCH_SINV_NR", (model, value) => { model.SimplifiedInvoiceNumber = value; return model; } },
        };
    }
}
