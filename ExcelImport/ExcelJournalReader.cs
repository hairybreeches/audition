using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Accounting;
using SqlImport;

namespace ExcelImport
{
    public class ExcelJournalReader
    {
        private readonly ExcelToSqlDataConverter toSqlDataConverter;
        private readonly SqlFinancialTransactionReader sqlFinancialTransactionReader;
        private readonly IDataReaderFactory readerFactory;

        public ExcelJournalReader(ExcelToSqlDataConverter toSqlDataConverter, SqlFinancialTransactionReader sqlFinancialTransactionReader, IDataReaderFactory readerFactory)
        {
            this.toSqlDataConverter = toSqlDataConverter;
            this.sqlFinancialTransactionReader = sqlFinancialTransactionReader;
            this.readerFactory = readerFactory;
        }

        public IEnumerable<Transaction> ReadJournals(ExcelImportMapping mapping)
        {
            var sheetReader = toSqlDataConverter.ReadSheet(mapping.SheetDescription);            
            return sqlFinancialTransactionReader.GetJournals(sheetReader, readerFactory.GetDataReader(mapping.Lookups));
        }
    }
}