using System;
using System.Data;

namespace SqlImport.DataReaders
{
    public class ColumnNameVerifierDecorator<T> : ISqlDataReader<T>
    {
        private readonly SchemaColumn<T> innerColumn;

        public ColumnNameVerifierDecorator(SchemaColumn<T> innerColumn)
        {
            this.innerColumn = innerColumn;
        }

        public T GetField(IDataRecord record, int recordIndex)
        {
            var actualFieldName = record.GetName(innerColumn.Index);
            if (actualFieldName != innerColumn.FieldName)
            {
                throw new SqlDataFormatUnexpectedException(
                    String.Format("Unrecognised data schema. Column {0} was {1}, expected {2}", 
                    innerColumn.Index, actualFieldName, innerColumn.FieldName));
            }

            return innerColumn.GetField(record, recordIndex);
        }
    }
}