using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser.Models
{
    public class CurrencyInfoModel : ICanParse
    {
        public string IDKey { get; set; }
        public string Name { get; set; }
        public int CurrencyType { get; set; }
        /// <summary>
        /// List of Tuple(int, double) representing exchange rate charged to customer.
        /// </summary>
        public List<(int, double)> ExchangeRate { get; set; } = new List<(int, double)>();
        public List<(int, double)> BaseExchangeRate { get; set; } = new List<(int, double)>();
        public List<(int, double)> CostPercentage { get; set; } = new List<(int, double)>();
        /// <summary>
        /// This field represents a reference for the back office for the cost charged to the 
        /// customer on the use of the foreign currency.
        /// </summary>
        public List<(int, string)> BOCRef { get; set; } = new List<(int, string)>();
        public int CurrencyRef { get; set; }
        public int LocalCurrency { get; set; }
        public int AmountNrDecimals { get; set; }
        public int FuelNrDecimals { get; set; }
        public int EVPriceNrDecimals { get; set; }
        public double SmallestCurrencyUnit { get; set; }

        public void AddToItem(string[] headers, string value)
        {
            switch (headers[0])
            {
                case "XRT":
                    (int, double) xrt = (int.Parse(headers[2]), double.Parse(value));
                    ExchangeRate.Add(xrt);
                    break;
                case "BASEXRT":
                    (int, double) basexrt = (int.Parse(headers[2]), double.Parse(value));
                    BaseExchangeRate.Add(basexrt);
                    break;
                default:
                    break;
            }
        }
    }
}
