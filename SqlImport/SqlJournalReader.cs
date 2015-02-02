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
            return journalCreator.ReadJournals(GetLineRecords(reader, dataReader));
        }

        private IEnumerable<SqlJournalLine> GetLineRecords(DataReader reader, JournalDataReader dataReader)
        {
            do
            {
                yield return journalLineParser.CreateJournalLine(reader.CurrentRecord(), dataReader, reader.RowNumber);
            } while (reader.Read());

        }
    }
}