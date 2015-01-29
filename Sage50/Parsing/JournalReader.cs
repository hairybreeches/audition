using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model.Accounting;
using SqlImport;

namespace Sage50.Parsing
{
    public class JournalReader
    {
        private readonly JournalLineParser journalLineParser;
        private readonly JournalSchema schema;

        public JournalReader(JournalLineParser journalLineParser, SageJournalSchema schema)
        {
            this.journalLineParser = journalLineParser;
            this.schema = schema;
        }

        public IEnumerable<Journal> GetJournals(IDataReader reader, NominalCodeLookup nominalCodeLookup)
        {
            return JournalParsing.ReadJournals(GetLineRecords(reader).Select(record => ConvertToLine(record, nominalCodeLookup)));
        }


        private IEnumerable<IDataRecord> GetLineRecords(IDataReader reader)
        {            
            while (reader.Read())
            {
                yield return reader;
            }
        }

        private SqlJournalLine ConvertToLine(IDataRecord record, NominalCodeLookup lookup)
        {
            var sqlJournalLine = journalLineParser.CreateJournalLine(record, schema);
            sqlJournalLine.NominalCodeName = lookup.GetNominalCodeName(sqlJournalLine.NominalCode);
            return sqlJournalLine;
        }
    }
}