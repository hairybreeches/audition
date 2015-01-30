using System.Data;

namespace SqlImport.Schema
{
    public class ToStringColumn : ISchemaColumn<string>
    {
        private readonly string fieldName;
        private readonly int index;

        public ToStringColumn(string fieldName, int index)
        {
            this.fieldName = fieldName;
            this.index = index;
        }

        public DataColumn ToDataColumn()
        {
            return new DataColumn(fieldName);
        }

        public int Index
        {
            get { return index; }
        }

        public string FieldName
        {
            get { return fieldName; }
        }

        public string GetField(IDataRecord record, int recordIndex)
        {
            return record.GetValue(index).ToString();
        }
    }
}