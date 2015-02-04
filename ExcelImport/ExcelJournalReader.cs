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
        private readonly FieldLookupInterpreter dataMapper;

        public ExcelJournalReader(ExcelDataConverter dataConverter, SqlJournalReader sqlJournalReader, FieldLookupInterpreter dataMapper)
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