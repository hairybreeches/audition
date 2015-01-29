using System;
using Model.Accounting;

namespace SqlImport
{
    public class SqlJournalLine
    {
        public int TransactionId { get; private set; }
        public string Username { get; private set; }
        public DateTime JournalDate { get; private set; }
        public DateTime CreationTime { get; private set; }
        public string NominalCode { get; private set; }
        public JournalType JournalType { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; }
        public string NominalCodeName { get; set; }

        public SqlJournalLine(int transactionId, string username, DateTime journalDate, DateTime creationTime, string nominalCode, decimal amount, JournalType type, String description, string nominalCodeName)
        {
            TransactionId = transactionId;
            Username = username;
            JournalDate = journalDate;
            CreationTime = creationTime;
            NominalCode = nominalCode;
            Amount = amount;
            JournalType = type;
            Description = description;
            NominalCodeName = nominalCodeName;
        }

        public override string ToString()
        {
            return
                String.Format(
                    "<Transaction number: {0}, username: {1}, date: {2} creation date: {3}, nominal code: {4}, Amount: {5} {6}>",
                    TransactionId, Username, JournalDate, CreationTime, NominalCode, Amount, JournalType);
        }
    }
}