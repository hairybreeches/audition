using System;
using System.Data;

namespace SqlImport.DataReaders
{
    public class ColumnNameVerifierDecorator<T> : IFieldReader<T>
    {
        private readonly string fieldName;
        private readonly int index;
        private readonly IFieldReader<T> dataReader;

        public ColumnNameVerifierDecorator(string fieldName, int index, IFieldReader<T> dataReader)
        {
            this.fieldName = fieldName;
            this.index = index;
            this.dataReader = dataReader;
        }

        public T GetField(IDataRecord record, int recordIndex)
        {
            var actualFieldName = record.GetName(index);
            if (actualFieldName != fieldName)
            {
                throw new SqlDataFormatUnexpectedException(
                    String.Format("Unrecognised data schema. Column {0} was {1}, expected {2}", 
                    index, actualFieldName, fieldName));
            }
            
            return dataReader.GetField(record, recordIndex);
        }
    }
}