using System.Collections.Generic;
using Model.Accounting;
using SqlImport;

namespace ExcelImport
{
    public class ExcelJournalReader
    {
        private readonly ExcelDataConverter dataConverter;
        private readonly SqlJournalReader sqlJournalReader;

        public ExcelJournalReader(ExcelDataConverter dataConverter, SqlJournalReader sqlJournalReader)
        {
            this.dataConverter = dataConverter;
            this.sqlJournalReader = sqlJournalReader;
        }

        public IEnumerable<Journal> ReadJournals(ExcelImportMapping importMapping)
        {
            var sheetReader = dataConverter.GetSheet(importMapping.SheetData).CreateDataReader();
            for (int i = 0; i < importMapping.SheetData.SkipRows; i++)
            {
                sheetReader.Read();
            }
            return sqlJournalReader.GetJournals(sheetReader, importMapping.Lookups.ToJournalSchema());
        }
    }
}