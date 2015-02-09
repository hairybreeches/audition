namespace ExcelImport
{

    public class FieldLookups
    {
        public FieldLookups(
            int description,
            int username,
            int created,
            int journalDate,
            int accountCode,
            int accountName,
            int amount,
            int id)
        {
            if (journalDate < 0)
            {
                throw new ExcelMappingException("The journal date must be mapped");
            }
            Description = description;
            Username = username;
            Created = created;
            JournalDate = journalDate;
            AccountCode = accountCode;
            AccountName = accountName;
            Amount = amount;
            Id = id;
        }

        public int Description { get; private set; }
        public int Username { get; private set; }
        public int Created { get; private set; }
        public int JournalDate { get; private set; }
        public int AccountCode { get; private set; }
        public int AccountName { get; private set; }
        public int Amount { get; private set; }
        public int Id { get; private set; }
    }
}