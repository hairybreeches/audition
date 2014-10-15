using System;
using System.Data;
using Model.Accounting;
using Sage50.Parsing.Schema;

namespace Sage50.Parsing
{
    /// <summary>
    /// Knows how to turn the raw IDataRecord from the db into a SageJournalLine
    /// Don't use this directly, use a JournalReader.
    /// </summary>
    public class JournalLineParser
    {
        private readonly JournalSchema schema;

        public JournalLineParser(JournalSchema schema)
        {
            this.schema = schema;
        }

        public SageJournalLine CreateJournalLine(IDataRecord record)
        {
            return CreateJournalLine(
                //dup
                schema.GetId(record),
                schema.GetUsername(record),
                schema.GetJournalDate(record),
                schema.GetCreationTime(record),
                schema.GetNominalCode(record), 
                schema.GetAmount(record),
                schema.GetDescription(record)
                );
        }

        private static SageJournalLine CreateJournalLine(int transactionId, string username, DateTime journalDate, DateTime creationTime, string nominalCode, double rawAmount, string description)
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

            return new SageJournalLine(transactionId, username, journalDate, creationTime, nominalCode, amount, type, description);
        }        
    }
}