namespace POSFileParser.Models
{
    public class MerchantInfoModel : ICanParse
    {
        public string IDKey { get; set; }
        public string MerchantID { get; set; }
        public string Name { get; set; }
        public int VATCode { get; set; }
        public string VATNumber { get; set; }

        public void AddToItem(string[] headers, string value)
        {
            switch (headers[0])
            {
                case "MERCH_ID":
                    MerchantID = value;
                    break;
                case "NAM":
                    Name = value;
                    break;
                case "VAT_ISO_C":
                    VATCode = int.Parse(value);
                    break;
                case "VAT_NR":
                    VATNumber = value;
                    break;
                default:
                    break;
            }
        }
    }
}
