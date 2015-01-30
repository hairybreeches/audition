using System;
using System.Data;

namespace SqlImport.Schema
{
    public class ColumnNameVerifierDecorator<T> : ISchemaColumn<T>
    {
        private readonly ISchemaColumn<T> column;

        public ColumnNameVerifierDecorator(ISchemaColumn<T> column)
        {
            this.column = column;
        }

        public DataColumn ToDataColumn()
        {
            return column.ToDataColumn();
        }

        public int Index
        {
            get { return column.Index; }
        }

        public string FieldName
        {
            get { return column.FieldName; }
        }

        public T GetField(IDataRecord record, int recordIndex)
        {
            var actualFieldName = record.GetName(Index);
            if (actualFieldName != FieldName)
            {
                throw new SqlDataFormatUnexpectedException(
                    String.Format("Unrecognised data schema at row {0}. Column {1} was {2}, expected {3}", 
                    recordIndex, Index, actualFieldName, FieldName));
            }

            return column.GetField(record, recordIndex);
        }
    }
}