using System;
using System.Data;

namespace SqlImport.Schema
{
    public class SchemaColumn<T> : ISqlDataReader<T>, ISchemaColumn
    {
        private readonly int index;
        private readonly string fieldName;

        public SchemaColumn(string fieldName, int index)
        {
            this.fieldName = fieldName;
            this.index = index;
        }

        public int Index
        {
            get { return index; }
        }

        public string FieldName
        {
            get { return fieldName; }
        }

        public DataColumn ToDataColumn()
        {
            return new DataColumn(FieldName, typeof(T));
        }

        public T GetField(IDataRecord record, int recordIndex)
        {
            var fieldValue = record[Index];

            if (!(fieldValue is T))
            {
                throw new SqlDataFormatUnexpectedException(
                    String.Format("Unrecognised data schema at row {0}. {1} was {2}, expected {3}", 
                    recordIndex, FieldName, fieldValue.GetType(), typeof(T)));
            }

            return (T)fieldValue;
        }
    }
}