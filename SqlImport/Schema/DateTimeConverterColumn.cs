using System;
using System.Data;

namespace SqlImport.Schema
{
    public class DateTimeConverterColumn : ISchemaColumn<DateTime>
    {
        private readonly SchemaColumn<string> inner;

        public DateTimeConverterColumn(string fieldName, int index)
        {
            inner = new SchemaColumn<string>(fieldName, index);
        }


        public DateTime GetField(IDataRecord record, int recordIndex)
        {
            return DateTime.Parse(inner.GetField(record, recordIndex));
        }

        public int Index
        {
            get { return inner.Index; }
        }

        public string FieldName
        {
            get { return inner.FieldName; }
        }

        public DataColumn ToDataColumn()
        {
            return inner.ToDataColumn();
        }
    }
}