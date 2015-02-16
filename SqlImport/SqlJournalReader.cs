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

        public SqlJournalReader(JournalLineParser journalLineParser, JournalCreator journalCreator)
        {
            this.journalLineParser = journalLineParser;
            this.journalCreator = journalCreator;
        }

        public IEnumerable<Transaction> GetJournals(SqlDataReader reader, JournalDataReader dataReader)
        {
            return journalCreator.ReadJournals(GetLineRecords(reader, dataReader));
        }

        private IEnumerable<SqlJournalLine> GetLineRecords(SqlDataReader reader, JournalDataReader dataReader)
        {
            while (reader.Read())
            {
                if (!reader.RowIsEmpty())
                {
                    yield return journalLineParser.CreateJournalLine(reader.CurrentRecord(), dataReader, reader.RowNumber);
                }                
            }
        }
    }
}