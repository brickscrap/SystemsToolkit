using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser.Models
{
    public class VatInfoModel : ICanParse
    {
        public string IDKey { get; set; }
        public double VATPerecentage { get; set; }

        public void AddToItem(string[] headers, string value)
        {
            VATPerecentage = double.Parse(value);
        }
    }
}
