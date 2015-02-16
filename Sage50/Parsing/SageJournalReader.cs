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
        private readonly SageTransactionSchema schema;

        public SageJournalReader(SageTransactionSchema schema, SqlJournalReader sqlJournalReader)
        {
            this.schema = schema;
            this.sqlJournalReader = sqlJournalReader;
        }

        public IEnumerable<Transaction> GetJournals(SqlDataReader sqlDataReader, NominalCodeLookup nominalLookup)
        {
            return sqlJournalReader.GetJournals(sqlDataReader, schema.CreateJournalReader(nominalLookup));
            
        }
    }
}