using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser.Models.TRX
{
    public class MerchantReceiptModel
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public int VATCountryCode { get; set; }
        public string VATNumber { get; set; }
        public int SequenceNumber { get; set; }
        public int VATSequenceNumber { get; set; }
        public string SimplifiedInvoiceNumber { get; set; }
    }
}
