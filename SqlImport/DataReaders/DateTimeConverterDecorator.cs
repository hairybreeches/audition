using System;
using System.Data;

namespace SqlImport.DataReaders
{
    public class DateTimeConverterDecorator : ISqlDataReader<DateTime>
    {
        private readonly ISqlDataReader<string> innerReader;

        public DateTimeConverterDecorator(ISqlDataReader<string> innerReader)
        {
            this.innerReader = innerReader;
        }

        public DateTime GetField(IDataRecord record, int recordIndex)
        {
            return DateTime.Parse(innerReader.GetField(record, recordIndex));
        }
    }
}