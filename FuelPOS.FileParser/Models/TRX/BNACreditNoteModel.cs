namespace POSFileParser.Models.TRX
{
    public class BNACreditNoteModel
    {
        /// <summary>
        /// The 6 digit note ID number of a BNA voucher being deleted.
        /// </summary>
        public int Identifier { get; set; }
        /// <summary>
        /// This field is available when a BNA credit note is deleted, or when 
        /// the amount of a BNA credit note is paid back to the customer at the till. 
        /// It provides the transaction number of the original BNA transaction for 
        /// which the credit note was issued.
        /// </summary>
        public string OriginalTrxNumber { get; set; }
        /// <summary>
        /// The amount of the credit note.
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// Whether or not the credit note number was revealed using an auth code.
        /// </summary>
        public bool CreditNoteRevealed { get; set; }
        /// <summary>
        /// Authorisation code used by the manager to reveal the credit note.
        /// </summary>
        public string AuthCode { get; set; }
    }
}
