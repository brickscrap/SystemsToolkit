using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser.Models
{
    public class HeaderReceiptModel : ICanParse
    {
        public string IDKey { get; set; }
        public string TextLine { get; set; }

        public void AddToItem(string[] headers, string value)
        {
            TextLine = value;
        }
    }
}
