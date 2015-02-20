using CsvHelper;

namespace CsvExport
{
    internal interface ICsvColumn<in TRecord>
    {
        void WriteField(ICsvWriter writer, TRecord record);
        void WriteHeader(ICsvWriter writer);
    }
}