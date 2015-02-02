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

        public ExcelJournalReader(ExcelDataConverter dataConverter, SqlJournalReader sqlJournalReader)
        {
            this.dataConverter = dataConverter;
            this.sqlJournalReader = sqlJournalReader;
        }

        public IEnumerable<Journal> ReadJournals(SheetMetadata sheetMetadata, ExcelDataMapper excelDataMapper)
        {
            var sheetReader = dataConverter.GetSheet(sheetMetadata).CreateDataReader();            
            do
            {
                if (!sheetReader.Read())
                {
                    return Enumerable.Empty<Journal>();
                }
            } while (sheetReader.RowIsEmpty());

            return sqlJournalReader.GetJournals(sheetReader, excelDataMapper.ToJournalSchema());
        }
    }
}