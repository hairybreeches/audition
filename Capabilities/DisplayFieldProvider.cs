using System.Collections.Generic;
using ExcelFormatting;

namespace Capabilities
{
    public class DisplayFieldProvider
    {
        public DisplayField TransactionDate = new DisplayField(DisplayFieldName.TransactionDate, MappingFields.TransactionDate, entry => entry.TransactionDate.ToString("yyyy-MM-dd"), "Transaction date", new DateColumnFormatter());
        public DisplayField Username = new DisplayField(DisplayFieldName.Username, MappingFields.Username, entry => entry.Username, "Username");
        public DisplayField Description = new DisplayField(DisplayFieldName.Description, MappingFields.Description, entry => entry.Description, "Description");
        public DisplayField LedgerEntryType = new DisplayField(DisplayFieldName.LedgerEntryType, MappingFields.Amount, entry => entry.LedgerEntryType, "Dr/Cr");
        public DisplayField NominalName = new DisplayField(DisplayFieldName.NominalName, MappingFields.NominalName, entry => entry.NominalCodeName, "Nominal name");
        public DisplayField Amount = new DisplayField(DisplayFieldName.Amount, MappingFields.Amount, entry => entry.Amount, "Amount");
        public DisplayField NominalCode = new DisplayField(DisplayFieldName.NominalCode, MappingFields.NominalCode, entry => entry.NominalCode, "Nominal Code");
        public DisplayField Id = new DisplayField(DisplayFieldName.Id, MappingFields.Id, entry => entry.TransactionId, "Transaction ID");
        public DisplayField TransactionType = new DisplayField(DisplayFieldName.Type, MappingFields.Type, entry => entry.TransactionType, "Transaction type");

        public IEnumerable<DisplayField> AllFields
        {
            get
            {
                yield return Id;
                yield return TransactionDate;
                yield return TransactionType;
                yield return Description;
                yield return Username;
                yield return NominalCode;
                yield return NominalName;
                yield return LedgerEntryType;
                yield return Amount;
            }
        }
    }
}