using System;
using System.Data;

namespace SqlImport.DataReaders
{
    public class DateTimeReader : ISqlDataReader<DateTime>
    {
        private readonly int columnIndex;
        private readonly string userFriendlyColumnName;

        public DateTimeReader(int columnIndex, string userFriendlyColumnName)
        {
            this.columnIndex = columnIndex;
            this.userFriendlyColumnName = userFriendlyColumnName;
        }

        public DateTime GetField(IDataRecord record, int recordIndex)
        {
            var value = record.GetValue(columnIndex);

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