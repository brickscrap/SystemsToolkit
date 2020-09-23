using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser.Models
{
    public class ArticleSoldInfoModel : ICanParse
    {
        public string IDKey { get; set; }
        public string Name { get; set; }
        public string ArticleNumber { get; set; }
        public string CardCode { get; set; }
        public string Group { get; set; }
        public string ReportGroup { get; set; }
        public int VatCode { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public void AddToItem(string[] headers, string value)
        {
            if (ArticleNumber == null)
            {
                ArticleNumber = headers[1];
            }
            switch (headers[0])
            {
                case "NAM":
                    Name = value;
                    return;
                case "CRD":
                    CardCode = value;
                    return;
                case "GRP":
                    Group = value;
                    return;
                case "REP":
                    ReportGroup = value;
                    return;
                case "VAT":
                    VatCode = int.Parse(value);
                    return;
                case "PRI":
                    Price = double.Parse(value);
                    return;
                case "QUAUNIT":
                    Quantity = int.Parse(value);
                    return;
                default:
                    return;
            }
        }
    }
}
