using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser.Models
{
    public class LionLoyaltyModel : ICanParse
    {
        public string IDKey { get; set; }
        public int TotalNrTransactions { get; set; }
        public double TotalDiscountAmt { get; set; }

        public void AddToItem(string[] headers, string value)
        {
            switch (headers[0])
            {
                case "TOTLOYHOSTDSCTRX":
                    TotalNrTransactions = int.Parse(value);
                    break;
                case "TOTLOYHOSTDSCAMT":
                    TotalDiscountAmt = double.Parse(value);
                    break;
                default:
                    break;
            }
        }
    }
}
