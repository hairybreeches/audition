using System;
using System.Data;

namespace SqlImport.DataReaders
{
    public class DefaultValueSupplier : IFieldReader<object>
    {
        private readonly object defaultValue;
        private readonly IFieldReader<object> inner;

        public DefaultValueSupplier(FieldReader fieldReader, object defaultValue)
        {
            this.defaultValue = defaultValue;
            inner = fieldReader;
        }

        public object GetField(IDataRecord record, int recordIndex)
        {
            var fieldValue = inner.GetField(record, recordIndex);

            if (fieldValue is DBNull)
            {
                return defaultValue;
            }

            return fieldValue;
        }
    }
}
