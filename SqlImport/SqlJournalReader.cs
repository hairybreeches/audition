using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model.Accounting;

namespace SqlImport
{
    public class SqlJournalReader
    {
        private readonly JournalLineParser journalLineParser;
        private readonly TransactionCreator transactionCreator;

        public SqlJournalReader(JournalLineParser journalLineParser, TransactionCreator transactionCreator)
        {
            this.journalLineParser = journalLineParser;
            this.transactionCreator = transactionCreator;
        }

        public IEnumerable<Transaction> GetJournals(SqlDataReader reader, JournalDataReader dataReader)
        {
            return transactionCreator.ReadTransactions(GetLineRecords(reader, dataReader));
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