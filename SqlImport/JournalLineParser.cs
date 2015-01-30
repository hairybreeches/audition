using System;
using System.Data;
using Model.Accounting;

namespace SqlImport
{
    /// <summary>
    /// Knows how to turn the raw IDataRecord from the db into a SqlJournalLine
    /// Don't use this directly, use a JournalReader.
    /// </summary>
    public class JournalLineParser
    {
        public SqlJournalLine CreateJournalLine(IDataRecord record, JournalSchema schema, int recordIndex)
        {
            return CreateJournalLine(
                schema.GetId(record, recordIndex),
                schema.GetUsername(record, recordIndex),
                schema.GetJournalDate(record, recordIndex),
                schema.GetCreationTime(record, recordIndex),
                schema.GetNominalCode(record, recordIndex),
                schema.GetAmount(record, recordIndex),
                schema.GetDescription(record, recordIndex),
                schema.GetNominalCodeName(record, recordIndex));
        }

        private static SqlJournalLine CreateJournalLine(string transactionId, string username, DateTime journalDate, DateTime creationTime, string nominalCode, double rawAmount, string description, string nominalCodeName)
        {
            JournalType type;
            decimal amount;

            if (rawAmount < 0)
            {
                type = JournalType.Cr;
                amount = -1 * (Decimal)rawAmount;
            }
            else
            {
                type = JournalType.Dr;
                amount = (Decimal)rawAmount;
            }

            return new SqlJournalLine(transactionId, username, journalDate, creationTime, nominalCode, amount, type, description, nominalCodeName);
        }        
    }
}