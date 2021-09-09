using FuelPOS.MutationCreator.Helpers;
using System.Collections.Generic;
using SysTk.DataManager.Models;

namespace FuelPOS.MutationCreator
{
    public static class CrdIdMut
    {
        public static void Create(List<CardIdentificationModel> cardIds, string outputPath)
        {
            List<string> output = new()
            {
                "[START_FILE]",
                "[CARD_IDENTIFICATION]"
            };

            for (int i = 0; i < cardIds.Count; i++)
            {
                output.Add($"PTTYP{i + 1}={cardIds[i].PaymentTerminalType}");
                output.Add($"NAM{i + 1}={cardIds[i].CardName}");
                output.Add($"TYPE{i + 1}={cardIds[i].CardType}");
                output.Add($"FROM{i + 1}={cardIds[i].FromRange}");
                output.Add($"TO{i + 1}={cardIds[i].ToRange}");
                output.Add($"START{i + 1}={cardIds[i].StartPosition}");
                output.Add($"LEN{i + 1}={cardIds[i].Length}");
                output.Add($"PANLEN{i + 1}={cardIds[i].PanLength}");
                if (cardIds[i].CardIdentifier is not null)
                {
                    output.Add($"CRDID{i + 1}={cardIds[i].CardIdentifier}");
                }
                output.Add($"CRDID_LEN{i + 1}={cardIds[i].CardIdentifierLength}");
                if (cardIds[i].AdditionalCardIdentifier is not null)
                {
                    output.Add($"ADDCRDID{i + 1}={cardIds[i].AdditionalCardIdentifier}");
                }
                output.Add($"ADDCRDID_LEN{i + 1}={cardIds[i].AdditionalCardIdentifierLength}");
                output.Add($"PRINTVAT{i + 1}={cardIds[i].PrintVAT.ToMutation()}");
                output.Add($"INVOICE{i + 1}={cardIds[i].InvoiceAllowed.ToMutation()}");
                output.Add($"PRINTVATNR{i + 1}={cardIds[i].PrintVATNumber.ToMutation()}");
                output.Add($"PRINTUPRI{i + 1}={cardIds[i].PrintUnitPrice.ToMutation()}");
                output.Add($"PRINTUQUA{i + 1}={cardIds[i].PrintQuantity.ToMutation()}");
                output.Add($"PRINTTOT{i + 1}={cardIds[i].PrintTotal.ToMutation()}");
                output.Add($"DELNOTE{i + 1}={cardIds[i].PrintDeliveryNote.ToMutation()}");
                output.Add($"LOYALL{i + 1}={cardIds[i].LoyaltyAllowed.ToMutation()}");
                output.Add($"PRINTEXPD{i + 1}={cardIds[i].PrintExpiryDate.ToMutation()}");
                output.Add($"MASKSTARTPOS{i + 1}={cardIds[i].StartMaskPosition}");
                output.Add($"MASKENDPOS{i + 1}={cardIds[i].EndMaskPosition}");
                output.Add($"MASKDIGIT{i + 1}={cardIds[i].MaskDigit}");
                output.Add($"AUTH_AMT{i + 1}={cardIds[i].AuthorisationAmount}");
                output.Add($"DEBITCARD{i + 1}={cardIds[i].DebitCard.ToMutation()}");
                output.Add($"SUPPRESSPANONTICKET{i + 1}={cardIds[i].SuppressPANOnTicket.ToMutation()}");
                output.Add($"SUPPRESSPANONRTXFILE{i + 1}={cardIds[i].SuppressPANOnRTXFile.ToMutation()}");
                output.Add($"PRESERVEHOSTAUTHAMOUNT{i + 1}={cardIds[i].PreserveHostAuthAmount.ToMutation()}");
                output.Add($"MPR_MAIN_APPLIC_TYPE{i + 1}={cardIds[i].MPRMainApplicationType}");
            }

            output.Add("[END_CARD_IDENTIFICATION]");
            output.Add("[END_FILE");

            FileCreator.Create(output, "CRDIDMUT.001", outputPath);
        }
    }
}
