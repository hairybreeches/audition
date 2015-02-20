using CsvHelper;

namespace CsvExport
{
    internal class NullCsvColumn<TRecord> : ICsvColumn<TRecord>
    {
        public void WriteField(ICsvWriter writer, TRecord record)
        {            
        }

        public void WriteHeader(ICsvWriter writer)
        {            
        }
    }
}