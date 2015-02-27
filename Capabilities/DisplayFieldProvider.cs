using System.Collections.Generic;

namespace Capabilities
{
    public class DisplayFieldProvider
    {
        public IEnumerable<DisplayField> GetAll
        {
            get
            {
                yield return new DisplayField(DisplayFieldName.TransactionDate, MappingFields.TransactionDate);
                yield return new DisplayField(DisplayFieldName.Description, MappingFields.Description);
                yield return new DisplayField(DisplayFieldName.Username, MappingFields.Username);
                yield return new DisplayField(DisplayFieldName.LedgerEntryType, MappingFields.Amount);
                yield return new DisplayField(DisplayFieldName.AccountName, MappingFields.NominalName);
                yield return new DisplayField(DisplayFieldName.Amount, MappingFields.Amount);
                yield return new DisplayField(DisplayFieldName.AccountCode, MappingFields.NominalCode);
                yield return new DisplayField(DisplayFieldName.Id, MappingFields.Id);   
                yield return new DisplayField(DisplayFieldName.Type, MappingFields.Type);
            }
        }
    }
}