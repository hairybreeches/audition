using System;
using System.Data;
using Model.Accounting;

namespace SqlImport
{
    /// <summary>
    /// Knows how to turn the raw IDataRecord into a SqlJournalLine with the help of a JournalDataReader 
    /// Don't use this directly, use a JournalReader.
    /// </summary>
    public class JournalLineParser
    {
        internal SqlJournalLine CreateJournalLine(IDataRecord record, TransactionFieldReader dataReader, int recordIndex)
        {
            return CreateJournalLine(
                dataReader.GetId(record, recordIndex),
                dataReader.GetUsername(record, recordIndex),
                dataReader.GetTransactionDate(record, recordIndex),
                dataReader.GetCreationTime(record, recordIndex),
                dataReader.GetNominalCode(record, recordIndex),
                dataReader.GetAmount(record, recordIndex),
                dataReader.GetDescription(record, recordIndex),
                dataReader.GetNominalCodeName(record, recordIndex));
        }

        private static SqlJournalLine CreateJournalLine(string transactionId, string username, DateTime transactionDate, DateTime creationTime, string nominalCode, double rawAmount, string description, string nominalCodeName)
        {
            LedgerEntryType type;
            decimal amount;

            if (rawAmount < 0)
            {
                type = LedgerEntryType.Cr;
                amount = -1 * (Decimal)rawAmount;
            }
            else
            {
                type = LedgerEntryType.Dr;
                amount = (Decimal)rawAmount;
            }

            return new SqlJournalLine(transactionId, username, transactionDate, creationTime, nominalCode, amount, type, description, nominalCodeName);
        }        
    }
}