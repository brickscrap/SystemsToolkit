using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser.Models
{
    public interface ICanParse
    {
        public string IDKey { get; set; }
        public void AddToItem(string[] headers, string value);
    }
}
