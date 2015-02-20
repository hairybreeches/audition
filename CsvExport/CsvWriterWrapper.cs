using CsvHelper;

namespace CsvExport
{
    public class CsvWriterWrapper : ISpreadsheetWriter
    {
        private readonly ICsvWriter csvWriter;

        public CsvWriterWrapper(ICsvWriter csvWriter)
        {
            this.csvWriter = csvWriter;
        }

        public void WriteField<T>(T field)
        {
            csvWriter.WriteField(field);
        }

        public void Dispose()
        {
            csvWriter.Dispose();
        }

        public void NextRecord()
        {
            csvWriter.NextRecord();
        }
    }
}