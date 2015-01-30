using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model.Accounting;
using SqlImport.Schema;

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
            return GetJournals(reader, schema, line => line);
        }
            
        public IEnumerable<Journal> GetJournals(IDataReader reader, JournalSchema schema, Func<SqlJournalLine, SqlJournalLine> adapter)
        {
            return JournalParsing.ReadJournals(GetLineRecords(reader).Select(record => ConvertToLine(record, schema, adapter)));
        }

        private SqlJournalLine ConvertToLine(IDataRecord record, JournalSchema schema, Func<SqlJournalLine, SqlJournalLine> adapter)
        {
            var sqlJournalLine = journalLineParser.CreateJournalLine(record, schema, recordIndex);
            recordIndex++;
            return adapter(sqlJournalLine);
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