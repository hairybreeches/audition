namespace CsvExport
{
    internal class NullCsvColumn<TRecord> : ICsvColumn<TRecord>
    {
        public void WriteField(ISpreadsheetWriter writer, TRecord record)
        {            
        }

        public void WriteHeader(ISpreadsheetWriter writer)
        {            
        }
    }
}