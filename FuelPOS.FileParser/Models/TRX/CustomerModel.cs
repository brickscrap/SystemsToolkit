namespace POSFileParser.Models.TRX
{
    public class CustomerModel
    {
        /// <summary>Number of the local account customer.</summary>
        public int Number { get; set; }
        /// <summary>Name of the local account customer.</summary>
        public string Name { get; set; }
        /// <summary>Identification number of the local account customer.</summary>
        public string Identifier { get; set; }
        /// <summary>Customer's additional information.</summary>
        public string Info { get; set; }
    }
}
