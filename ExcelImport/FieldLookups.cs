using System;
using SqlImport;

namespace ExcelImport
{

    public class FieldLookups
    {
        public FieldLookups(
            int description,
            int username,
            int created,
            int transactionDate,
            int accountCode,
            int accountName,
            int amount,
            int id, 
            int type)
        {
            if (transactionDate < 0)
            {
                throw new ExcelMappingException(String.Format("The {0} must be mapped", MappingField.TransactionDate));
            }
            Description = description;
            Username = username;
            Created = created;
            TransactionDate = transactionDate;
            AccountCode = accountCode;
            AccountName = accountName;
            Amount = amount;
            Id = id;
            Type = type;
        }

        public int Description { get; private set; }
        public int Username { get; private set; }
        public int Created { get; private set; }
        public int TransactionDate { get; private set; }
        public int AccountCode { get; private set; }
        public int AccountName { get; private set; }
        public int Amount { get; private set; }
        public int Id { get; private set; }
        public int Type { get; private set; }
    }
}