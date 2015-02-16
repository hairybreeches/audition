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
            int id)
        {
            if (transactionDate < 0)
            {
                throw new ExcelMappingException("The transaction date must be mapped");
            }
            Description = description;
            Username = username;
            Created = created;
            TransactionDate = transactionDate;
            AccountCode = accountCode;
            AccountName = accountName;
            Amount = amount;
            Id = id;
        }

        public int Description { get; private set; }
        public int Username { get; private set; }
        public int Created { get; private set; }
        public int TransactionDate { get; private set; }
        public int AccountCode { get; private set; }
        public int AccountName { get; private set; }
        public int Amount { get; private set; }
        public int Id { get; private set; }
    }
}