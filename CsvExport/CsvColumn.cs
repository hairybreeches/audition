using System;
using CsvHelper;

namespace CsvExport
{
    internal class CsvColumn<TOutput> : ICsvColumn<TOutput>
    {
        private readonly string header;
        private readonly Func<TOutput, object> fieldSelector;

        public CsvColumn(string header, Func<TOutput, object> fieldSelector)
        {
            this.header = header;
            this.fieldSelector = fieldSelector;
        }

        public void WriteField(ICsvWriter writer, TOutput record)
        {
            writer.WriteField(fieldSelector(record));
        }

        public void WriteHeader(ICsvWriter writer)
        {
            writer.WriteField(header);
        }
    }
}