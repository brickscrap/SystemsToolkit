using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace POSFileParser.Models
{
    public class FuelInfoModel : ICanParse
    {
        public string IDKey { get; set; }
        public string Name { get; set; }
        public int CardCode { get; set; }
        public int Group { get; set; }
        public int ReportCode { get; set; }
        public int VatCode { get; set; }
        public int UnitQuantity { get; set; }
        public int Bfl { get; set; }
        public int BlendRatio { get; set; }
        public string ExternalRef { get; set; }
        public double BasePrice { get; set; }
        public List<double> AltPrices { get; set; } = new List<double>();

        public void AddToItem(string[] headers, string value)
        {
            switch (headers[0])
            {
                case "NAM":
                    Name = value;
                    break;
                case "CRD":
                    CardCode = int.Parse(value);
                    break;
                case "GRP":
                    Group = int.Parse(value);
                    break;
                case "REP":
                    ReportCode = int.Parse(value);
                    break;
                case "VAT":
                    VatCode = int.Parse(value);
                    break;
                case "QUAUNIT":
                    UnitQuantity = int.Parse(value);
                    break;
                case "BFL":
                    Bfl = int.Parse(value);
                    break;
                case "BRATIO":
                    BlendRatio = int.Parse(value);
                    break;
                case "BASEPRI":
                    BasePrice = double.Parse(value);
                    break;
                case "ALTPRI":
                    AltPrices.Add(double.Parse(value));
                    break;
                case "EXTREF":
                    ExternalRef = value;
                    break;
                default:
                    break;
            }
        }
    }
}
