using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Accounting;
using SqlImport;

namespace ExcelImport
{
    public class ExcelJournalReader
    {
        private readonly ExcelDataConverter dataConverter;
        private readonly SqlJournalReader sqlJournalReader;
        private readonly ExcelDataMapper dataMapper;

        public ExcelJournalReader(ExcelDataConverter dataConverter, SqlJournalReader sqlJournalReader, ExcelDataMapper dataMapper)
        {
            this.dataConverter = dataConverter;
            this.sqlJournalReader = sqlJournalReader;
            this.dataMapper = dataMapper;
        }

        public IEnumerable<Journal> ReadJournals(ExcelImportMapping mapping)
        {
            var sheetReader = dataConverter.ReadSheet(mapping.SheetData);            
            return sqlJournalReader.GetJournals(sheetReader, dataMapper.GetDataReader(mapping.Lookups));
        }
    }
}