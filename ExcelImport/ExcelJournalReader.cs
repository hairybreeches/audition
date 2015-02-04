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
        private readonly SqlJournalReader sqlJournalReader;
        private readonly IDataReaderFactory readerFactory;

        public ExcelJournalReader(ExcelToSqlDataConverter toSqlDataConverter, SqlJournalReader sqlJournalReader, IDataReaderFactory readerFactory)
        {
            this.toSqlDataConverter = toSqlDataConverter;
            this.sqlJournalReader = sqlJournalReader;
            this.readerFactory = readerFactory;
        }

        public IEnumerable<Journal> ReadJournals(ExcelImportMapping mapping)
        {
            var sheetReader = toSqlDataConverter.ReadSheet(mapping.SheetData);            
            return sqlJournalReader.GetJournals(sheetReader, readerFactory.GetDataReader(mapping.Lookups));
        }
    }
}