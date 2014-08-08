namespace Model
{
    public class JournalLine
    {
        public JournalLine(string accountCode, string accountName, JournalType journalType, decimal amount)
        {
            AccountCode = accountCode;
            AccountName = accountName;
            JournalType = journalType;
            Amount = amount;
        }

        public string AccountCode { get; private set; }
        public string AccountName { get; private set; }        
        public JournalType JournalType { get; private set; }
        public decimal Amount { get; private set; }
    }
}
