namespace CsvExport
{
    public interface ISpreadsheetWriterFactory
    {
        ISpreadsheetWriter CreateWriter(string filename);
    }
}