using System;
using System.Data;
using SqlImport.Schema;

namespace ExcelImport
{
    public class DateTimeConverterColumn : ISchemaColumn<DateTime>
    {
        private readonly SchemaColumn<string> inner;

        public DateTimeConverterColumn(string fieldName, int index)
        {
            inner = new SchemaColumn<string>(fieldName, index);
        }


        public DateTime GetField(IDataRecord record)
        {
            return DateTime.Parse(inner.GetField(record));
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