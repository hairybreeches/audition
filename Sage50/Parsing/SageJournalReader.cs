using System.Collections.Generic;
using System.Data;
using Model;
using Model.Accounting;
using SqlImport;

namespace Sage50.Parsing
{
    public class SageJournalReader
    {
        
        private readonly SqlJournalReader sqlJournalReader;
        private readonly SageJournalSchema schema;

        public SageJournalReader(SageJournalSchema schema, SqlJournalReader sqlJournalReader)
        {
            this.schema = schema;
            this.sqlJournalReader = sqlJournalReader;
        }

        public IEnumerable<Journal> GetJournals(IDataReader dataReader, NominalCodeLookup nominalLookup)
        {
            if (dataReader.Read())
            {
                return sqlJournalReader.GetJournals(dataReader, schema.CreateJournalReader(nominalLookup));
            }
            else
            {
                throw new NoJournalsException("Successfully opened the accounts in Sage, but the appear to have no entries");
            }
            
        }
    }
}