using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTk.DataManager.Models
{
    public record CardIdentificationModel
    {
        public int PaymentTerminalType { get; set; }
        public int Sequence { get; set; }
        public string CardName { get; set; }
        public int CardType { get; set; }
        public string CardIdentifier { get; set; }
        public int CardIdentifierLength { get; set; }
        public string AdditionalCardIdentifier { get; set; }
        public int AdditionalCardIdentifierLength { get; set; }
        public string FromRange { get; set; }
        public string ToRange { get; set; }
        public int StartPosition { get; set; }
        public int Length { get; set; }
        public int PanLength { get; set; }
        public bool PrintVAT { get; set; }
        public bool InvoiceAllowed { get; set; }
        public bool PrintVATNumber { get; set; }
        public bool PrintUnitPrice { get; set; }
        public bool PrintQuantity { get; set; }
        public bool PrintTotal { get; set; }
        public bool PrintDeliveryNote { get; set; }
        public bool LoyaltyAllowed { get; set; }
        public bool PrintExpiryDate { get; set; }
        public int StartMaskPosition { get; set; }
        public int EndMaskPosition { get; set; }
        public string MaskDigit { get; set; }
        public int AuthorisationAmount { get; set; }
        public bool DebitCard { get; set; }
        public bool SuppressPANOnTicket { get; set; }
        public bool SuppressPANOnRTXFile { get; set; }
        public bool PreserveHostAuthAmount { get; set; }
        public int MPRMainApplicationType { get; set; }
        public bool ShowPreAuthAmountOnOPT { get; set; }
        public string MopIdForLoyalty { get; set; }
    }
}
