using System;
using System.Data;

namespace SqlImport
{
    public class UnmappedColumn<T> : ISchemaColumn<T>
    {
        public T GetField(IDataRecord record)
        {
            return default(T);
        }

        public DataColumn ToDataColumn()
        {
            return null;
        }

        public int Index
        {
            get { return -1; }
        }

        public string FieldName
        {
            get { return String.Empty; }
        }
    }
}