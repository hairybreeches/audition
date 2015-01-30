using System;
using System.Data;

namespace SqlImport.DataReaders
{
    public class DateTimeReader : ISqlDataReader<DateTime>
    {
        private readonly int columnIndex;

        public DateTimeReader(int columnIndex)
        {
            this.columnIndex = columnIndex;
        }

        public DateTime GetField(IDataRecord record, int recordIndex)
        {
            var value = record.GetValue(columnIndex);

            if (value is DateTime)
            {
                return (DateTime) value;
            }

            if (value is string)
            {
                return DateTime.Parse(value as string);
            }

            if (value is double)
            {
                return DateTime.FromOADate((double) value);
            }
            
            throw new SqlDataFormatUnexpectedException(String.Format("Could not interpret {0} as a date, column {1} row {2}", value, columnIndex, recordIndex));
        }
    }
}