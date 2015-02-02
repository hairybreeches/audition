using System;
using System.Data;

namespace SqlImport.DataReaders
{
    public class SqlDataReader<T> : ISqlDataReader<T>
    {
        private readonly int index;

        public SqlDataReader(int index)
        {
            this.index = index;
        }

        public T GetField(IDataRecord record, int recordIndex)
        {
            var fieldValue = record[index];

            if (!(fieldValue is T))
            {
                throw new SqlDataFormatUnexpectedException(
                    String.Format("Unrecognised data schema. Value '{0}' from column {1} was {2}, expected {3}", 
                    fieldValue, index, fieldValue.GetType(), typeof(T)));
            }

            return (T)fieldValue;
        }
    }
}