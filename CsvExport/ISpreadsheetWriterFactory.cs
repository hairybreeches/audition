using CsvHelper;

namespace CsvExport
{
    public interface ISpreadsheetWriterFactory
    {
        ISpreadsheetWriter CreateWriter(string filename);
    }
}