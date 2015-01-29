using System;
using System.Data;

namespace SqlImport.Schema
{
    public class SchemaColumn<T> : ISchemaColumn<T>
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

        public T GetField(IDataRecord record)
        {
            var fieldValue = record[Index];

            if (!(fieldValue is T))
            {
                throw new SqlDataFormatUnexpectedException(
                    String.Format("Unrecognised data schema. {0} was {1}, expected {2}", FieldName,
                        fieldValue.GetType(), typeof(T)));
            }

            return (T)fieldValue;
        }
    }
}