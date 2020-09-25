using POSFileParser.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser.Models.TRX
{
    public class HostDetailModel
    {
        public HostType HostType { get; set; }
        /// <summary>
        /// In case of an outdoor mobile payment transaction, this field indicates the mobile 
        /// payment provider that initiated the transaction.
        /// </summary>
        public HostCode Code { get; set; }
        /// <summary>
        /// If the host uses a reference number for the period between two closures, and if 
        /// this reference number is known by Fuel POS, then it is provided in this field.
        /// </summary>
        public int PeriodNumber { get; set; }
        public string Reference { get; set; }
        public string CAIC { get; set; }
    }
}
