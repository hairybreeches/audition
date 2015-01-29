using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model.Accounting;

namespace SqlImport
{
    public class SqlJournalReader
    {
        private readonly JournalLineParser journalLineParser;

        public SqlJournalReader(JournalLineParser journalLineParser)
        {
            this.journalLineParser = journalLineParser;
        }

        public IEnumerable<Journal> GetJournals(IDataReader reader, JournalSchema schema, Func<SqlJournalLine, SqlJournalLine> adapter)
        {
            return JournalParsing.ReadJournals(GetLineRecords(reader).Select(record => ConvertToLine(record, schema, adapter)));
        }

        private SqlJournalLine ConvertToLine(IDataRecord record, JournalSchema schema, Func<SqlJournalLine, SqlJournalLine> adapter)
        {
            var sqlJournalLine = journalLineParser.CreateJournalLine(record, schema);
            return adapter(sqlJournalLine);
        }

        private static IEnumerable<IDataRecord> GetLineRecords(IDataReader reader)
        {
            while (reader.Read())
            {
                yield return reader;
            }
        }
    }
}