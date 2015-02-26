using System;
using System.Data;

namespace SqlImport.DataReaders
{
    public class TypedDataReader<T> : IFieldReader<T>
    {
        private readonly string userFriendlyColumnName;
        private readonly IFieldReader<object> inner;

        public TypedDataReader(int index, string userFriendlyColumnName)
        {
            inner = new FieldReader(index);
            this.userFriendlyColumnName = userFriendlyColumnName;
        }

        public T GetField(IDataRecord record, int recordIndex)
        {
            var fieldValue = inner.GetField(record, recordIndex);

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