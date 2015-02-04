using System;
using System.Data;

namespace SqlImport.DataReaders
{
    public class SqlDataReader<T> : IFieldReader<T>
    {
        private readonly int index;
        private readonly string userFriendlyColumnName;

        public SqlDataReader(int index, string userFriendlyColumnName)
        {
            this.index = index;
            this.userFriendlyColumnName = userFriendlyColumnName;
        }

        public T GetField(IDataRecord record, int recordIndex)
        {
            var fieldValue = record[index];

            if (!(fieldValue is T))
            {
                throw new SqlDataFormatUnexpectedException(
                    String.Format("Unrecognised data schema. Value '{0}' from column {1} was {2}, expected {3}", 
                    fieldValue, userFriendlyColumnName, fieldValue.GetType(), typeof(T)));
            }

            return (T)fieldValue;
        }
    }
}