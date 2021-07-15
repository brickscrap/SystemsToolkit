using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser.Models.TRX
{
    public class PaymentVoucherModel
    {
        /// <summary>
        /// The number of an automatically deleted payment voucher.
        /// </summary>
        public string Identifier { get; set; }
        public double Amount { get; set; }
        public double Quantity { get; set; }
    }
}
