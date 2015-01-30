using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model.Accounting;

namespace SqlImport
{
    public class SqlJournalReader
    {
        private readonly JournalLineParser journalLineParser;
        private int recordIndex = 0;

        public SqlJournalReader(JournalLineParser journalLineParser)
        {
            this.journalLineParser = journalLineParser;
        }

        public IEnumerable<Journal> GetJournals(IDataReader reader, JournalSchema schema)
        {
            return JournalParsing.ReadJournals(GetLineRecords(reader).Select(record => ConvertToLine(record, schema)));
        }

        private SqlJournalLine ConvertToLine(IDataRecord record, JournalSchema schema)
        {
            var sqlJournalLine = journalLineParser.CreateJournalLine(record, schema, recordIndex);
            recordIndex++;
            return sqlJournalLine;
        }

        private static IEnumerable<IDataRecord> GetLineRecords(IDataReader reader)
        {
            do
            {
                yield return reader;
            } while (reader.Read());

        }
    }
}