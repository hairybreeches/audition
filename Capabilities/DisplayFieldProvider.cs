using System.Collections.Generic;

namespace Capabilities
{
    public class DisplayFieldProvider
    {
        public DisplayField TransactionDate = new DisplayField(DisplayFieldName.TransactionDate, MappingFields.TransactionDate, entry => entry.TransactionDate.ToString("yyyy-MM-dd"));
        public DisplayField Username = new DisplayField(DisplayFieldName.Username, MappingFields.Username, entry => entry.Username);
        public DisplayField Description = new DisplayField(DisplayFieldName.Description, MappingFields.Description, entry => entry.Description);
        public DisplayField LedgerEntryType = new DisplayField(DisplayFieldName.LedgerEntryType, MappingFields.Amount, entry => entry.LedgerEntryType);
        public DisplayField NominalName = new DisplayField(DisplayFieldName.NominalName, MappingFields.NominalName, entry => entry.NominalCodeName);
        public DisplayField Amount = new DisplayField(DisplayFieldName.Amount, MappingFields.Amount, entry => entry.Amount);
        public DisplayField NominalCode = new DisplayField(DisplayFieldName.NominalCode, MappingFields.NominalCode, entry => entry.NominalCode);
        public DisplayField Id = new DisplayField(DisplayFieldName.Id, MappingFields.Id, entry => entry.TransactionId);
        public DisplayField TransactionType = new DisplayField(DisplayFieldName.Type, MappingFields.Type, entry => entry.TransactionType);
        public DisplayField AccountCode = new DisplayField(DisplayFieldName.AccountCode, MappingFields.AccountCode, entry => entry.AccountCode);

        public IEnumerable<DisplayField> AllFields
        {
            get
            {
                yield return TransactionDate;
                yield return Description;
                yield return Username;
                yield return LedgerEntryType;
                yield return NominalName;
                yield return Amount;
                yield return NominalCode;
                yield return Id;
                yield return TransactionType;
                yield return AccountCode;
            }
        }
    }
}