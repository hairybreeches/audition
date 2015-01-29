using System;
using System.Data;
using SqlImport;
using SqlImport.Schema;

namespace Sage50.Parsing
{
    internal class ColumnNameVerifierDecorator<T> : ISchemaColumn<T>
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

        public T GetField(IDataRecord record)
        {
            var actualFieldName = record.GetName(Index);
            if (actualFieldName != FieldName)
            {
                throw new SqlDataFormatUnexpectedException(
                    String.Format("Unrecognised data schema. Column {0} was {1}, expected {2}", Index,
                        actualFieldName, FieldName));
            }

            return column.GetField(record);
        }
    }
}