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
                    String.Format("Unrecognised data schema at row {0}. Column {1} was {2}, expected {3}",
                    recordIndex, innerColumn.Index, actualFieldName, innerColumn.FieldName));
            }

            return innerColumn.GetField(record, recordIndex);
        }
    }
}