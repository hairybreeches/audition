using System;
using System.Data;

namespace SqlImport.DataReaders
{
    public class SqlDataReader<T> : ISqlDataReader<T>
    {
        private readonly string fieldName;
        private readonly int index;

        public SqlDataReader(string fieldName, int index)
        {
            this.fieldName = fieldName;
            this.index = index;
        }

        public T GetField(IDataRecord record, int recordIndex)
        {
            var fieldValue = record[index];

            if (!(fieldValue is T))
            {
                throw new SqlDataFormatUnexpectedException(
                    String.Format("Unrecognised data schema at row {0}. {1} was {2}, expected {3}",
                        recordIndex, fieldName, fieldValue.GetType(), typeof(T)));
            }

            return (T)fieldValue;
        }
    }
}