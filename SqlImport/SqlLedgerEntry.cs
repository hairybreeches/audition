using System;
using Model.Accounting;

namespace SqlImport
{
    /// <summary>
    /// This is the format Sage stores the data in, and outputs it to Excel.
    /// It's also the format we use to output data to Excel
    /// It seems like the format accountants expect when viewing entries in a tabular format
    /// </summary>
    public class SqlLedgerEntry
    {
        public string TransactionId { get; private set; }
        public string Username { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public DateTime CreationTime { get; private set; }
        public string NominalCode { get; private set; }
        public LedgerEntryType LedgerEntryType { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; }
        public string NominalCodeName { get; private set; }
        public string TransactionType { get; private set; }

        public SqlLedgerEntry(string transactionId, string username, DateTime transactionDate, DateTime creationTime, string nominalCode, decimal amount, LedgerEntryType type, string description, string nominalCodeName, string transactionType)
        {
            TransactionId = transactionId;
            Username = username;
            TransactionDate = transactionDate;
            CreationTime = creationTime;
            NominalCode = nominalCode;
            Amount = amount;
            LedgerEntryType = type;
            Description = description;
            NominalCodeName = nominalCodeName;
            TransactionType = transactionType;
        }

        public override string ToString()
        {
            return
                String.Format(
                    "<Transaction number: {0}, username: {1}, date: {2} creation date: {3}, nominal code: {4}, Amount: {5} {6}>",
                    TransactionId, Username, TransactionDate, CreationTime, NominalCode, Amount, LedgerEntryType);
        }
    }
}