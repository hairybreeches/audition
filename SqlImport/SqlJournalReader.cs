using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model.Accounting;

namespace SqlImport
{
    public class SqlJournalReader
    {
        private readonly JournalLineParser journalLineParser;
        private readonly JournalCreator journalCreator;
        private int recordIndex = 0;

        public SqlJournalReader(JournalLineParser journalLineParser, JournalCreator journalCreator)
        {
            this.journalLineParser = journalLineParser;
            this.journalCreator = journalCreator;
        }

        public IEnumerable<Journal> GetJournals(DataReader reader, JournalDataReader dataReader)
        {
            return journalCreator.ReadJournals(GetLineRecords(reader).Select(record => ConvertToLine(record, dataReader)));
        }

        private SqlJournalLine ConvertToLine(IDataRecord record, JournalDataReader dataReader)
        {
            var sqlJournalLine = journalLineParser.CreateJournalLine(record, dataReader, recordIndex);
            recordIndex++;
            return sqlJournalLine;
        }

        private static IEnumerable<IDataRecord> GetLineRecords(DataReader reader)
        {
            do
            {
                yield return reader.CurrentRecord();
            } while (reader.Read());

        }
    }
}