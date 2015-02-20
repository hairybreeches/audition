using CsvHelper;

namespace CsvExport
{
    public interface ISpreadsheetWriterFactory
    {
        ICsvWriter CreateWriter(string filename);
    }
}