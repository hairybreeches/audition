using System;
using System.Data;
using Model.Accounting;
using Sage50.Parsing.Schema;
using SqlImport;

namespace Sage50.Parsing
{
    /// <summary>
    /// Knows how to turn the raw IDataRecord from the db into a SqlJournalLine
    /// Don't use this directly, use a JournalReader.
    /// </summary>
    public class JournalLineParser
    {
        private readonly JournalSchema schema;

        public JournalLineParser(JournalSchema schema)
        {
            this.schema = schema;
        }

        public SqlJournalLine CreateJournalLine(IDataRecord record, NominalCodeLookup lookup)
        {
            var nominalCode = schema.GetNominalCode(record);
            return CreateJournalLine(
                schema.GetId(record),
                schema.GetUsername(record),
                schema.GetJournalDate(record),
                schema.GetCreationTime(record),
                nominalCode, 
                schema.GetAmount(record),
                schema.GetDescription(record),
                lookup.GetNominalCodeName(nominalCode));
        }

        private static SqlJournalLine CreateJournalLine(int transactionId, string username, DateTime journalDate, DateTime creationTime, string nominalCode, double rawAmount, string description, string nominalCodeName)
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