using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model.Accounting;

namespace SqlImport
{
    public class SqlFinancialTransactionReader
    {
        private readonly LedgerEntryParser ledgerEntryParser;
        private readonly TransactionCreator transactionCreator;

        public SqlFinancialTransactionReader(LedgerEntryParser ledgerEntryParser, TransactionCreator transactionCreator)
        {
            this.ledgerEntryParser = ledgerEntryParser;
            this.transactionCreator = transactionCreator;
        }

        public IEnumerable<Transaction> GetJournals(SqlDataReader reader, TransactionFieldReader dataReader)
        {
            return transactionCreator.ReadTransactions(GetLineRecords(reader, dataReader));
        }

        private IEnumerable<SqlLedgerEntry> GetLineRecords(SqlDataReader reader, TransactionFieldReader dataReader)
        {
            while (reader.Read())
            {
                if (!reader.RowIsEmpty())
                {
                    yield return ledgerEntryParser.CreateLedgerEntry(reader.CurrentRecord(), dataReader, reader.RowNumber);
                }                
            }
        }
    }
}