using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public IEnumerable<Journal> GetJournals(DataReader dataReader, NominalCodeLookup nominalLookup)
        {
            return sqlJournalReader.GetJournals(dataReader, schema.CreateJournalReader(nominalLookup));
            
        }
    }
}