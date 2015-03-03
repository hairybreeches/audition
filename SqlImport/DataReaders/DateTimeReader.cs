using System;
using System.Data;

namespace SqlImport.DataReaders
{
    public class DateTimeReader : IFieldReader<DateTime>
    {
        private readonly string userFriendlyColumnName;
        private readonly IFieldReader<object> inner;

        public DateTimeReader(int columnIndex, string userFriendlyColumnName)
        {
            inner = new FieldReader(columnIndex);
            this.userFriendlyColumnName = userFriendlyColumnName;
        }

        public DateTime GetField(IDataRecord record, int recordIndex)
        {
            var value = inner.GetField(record, recordIndex);

            if (value is DateTime)
            {
                return (DateTime) value;
            }

            DateTime dateTimeParse;
            if (value is string && DateTime.TryParse(value as string, out dateTimeParse))
            {
                return dateTimeParse;
            }

            if (value is double)
            {
                return DateTime.FromOADate((double) value);
            }
            
            throw new SqlDataFormatUnexpectedException(String.Format("Could not interpret value '{0}' from column {1} as a date", value, userFriendlyColumnName));
        }
    }
}