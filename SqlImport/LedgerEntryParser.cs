using System;
using System.Data;
using Model;
using Model.Accounting;

namespace SqlImport
{
    /// <summary>
    /// Knows how to turn the raw IDataRecord into a SqlJournalLine with the help of a JournalDataReader 
    /// Don't use this directly, use a JournalReader.
    /// </summary>
    public class LedgerEntryParser
    {
        internal SqlLedgerEntry CreateLedgerEntry(IDataRecord record, TransactionFieldReader dataReader, int recordIndex)
        {
            return CreateLedgerEntry(
                dataReader.GetId(record, recordIndex),
                dataReader.GetUsername(record, recordIndex),
                dataReader.GetTransactionDate(record, recordIndex),
                dataReader.GetCreationTime(record, recordIndex),
                dataReader.GetNominalCode(record, recordIndex),
                dataReader.GetAmount(record, recordIndex),
                dataReader.GetDescription(record, recordIndex),
                dataReader.GetNominalCodeName(record, recordIndex),
                dataReader.GetType(record, recordIndex));
        }

        private static SqlLedgerEntry CreateLedgerEntry(string transactionId, string username, DateTime transactionDate, DateTime creationTime, string nominalCode, double rawAmount, string description, string nominalCodeName, string transactionType)
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

            return new SqlLedgerEntry(transactionId, username, transactionDate, creationTime, nominalCode, amount, type, description, nominalCodeName, transactionType);
        }        
    }
}