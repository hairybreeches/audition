using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Model;
using Model.Accounting;
using SqlImport;

namespace Sage50.Parsing
{
    public class SageJournalReader
    {
        
        private readonly JournalSchema schema;
        private readonly SqlJournalReader sqlJournalReader;

        public SageJournalReader(SageJournalSchema schema, SqlJournalReader sqlJournalReader)
        {
            this.schema = schema;
            this.sqlJournalReader = sqlJournalReader;
        }


        private SqlJournalLine AddNominalCode(SqlJournalLine journalLine, NominalCodeLookup lookup)
        {
            //a little cheeky, that we mutate rather than creating a new object here, but there could be a lot of these
            //and the SqlJournalLine is an "in progress" object
            journalLine.NominalCodeName = lookup.GetLookupValue(journalLine.NominalCode);
            return journalLine;
        }

        public IEnumerable<Journal> GetJournals(IDataReader dataReader, NominalCodeLookup nominalLookup)
        {
            if (dataReader.Read())
            {
                return sqlJournalReader.GetJournals(dataReader, schema, line => AddNominalCode(line, nominalLookup));
            }
            else
            {
                throw new NoJournalsException("Successfully opened the accounts in Sage, but the appear to have no entries");
            }
            
        }
    }
}