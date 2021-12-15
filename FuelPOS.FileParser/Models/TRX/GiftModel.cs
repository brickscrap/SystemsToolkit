using System;

namespace POSFileParser.Models.TRX
{
    public class GiftModel : ICanParse
    {
        public string IDKey { get; set; }
        /// <summary>
        /// Barcode of the loyalty gift
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// This field gives the number of loyalty points that were redeemed to obtain the 
        /// special loyalty gift which is provided in the field GIFTBARx,t.
        /// Item1 is the identifier, t. Item2 is the amount of points redeemed.
        /// </summary>
        public double PointsRedeemed { get; set; }
        /// <summary>
        /// This field gives the additional amount that the customer needed to pay to obtain
        /// the special loyalty gift which is provided in the field GIFTBARx,t.
        /// Item1 is the identifier, t. Item2 is the amount.
        /// </summary>
        public double AdditionalAmount { get; set; }
        /// <summary>
        /// Name of the loyalty gift.
        /// </summary>
        public string Name { get; set; }


        public void AddToItem(string[] headers, string value)
        {
            throw new NotImplementedException();
        }
    }
}
