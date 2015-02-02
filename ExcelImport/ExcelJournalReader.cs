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
            var sheetReader = dataConverter.ReadSheet(sheetMetadata);            
            do
            {
                if (!sheetReader.Read())
                {
                    return Enumerable.Empty<Journal>();
                }
            } while (sheetReader.CurrentRecord().RowIsEmpty());

            return sqlJournalReader.GetJournals(sheetReader, excelDataMapper.GetDataReader());
        }
    }
}