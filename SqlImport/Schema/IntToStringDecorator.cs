using System.Data;

namespace SqlImport.Schema
{
    public class IntToStringDecorator : ISchemaColumn<string>
    {
        private readonly ISchemaColumn<int> innerColumn;

        public IntToStringDecorator(ISchemaColumn<int> innerColumn)
        {
            this.innerColumn = innerColumn;
        }

        public DataColumn ToDataColumn()
        {
            return innerColumn.ToDataColumn();
        }

        public int Index
        {
            get { return innerColumn.Index; }
        }

        public string FieldName
        {
            get { return innerColumn.FieldName; }
        }

        public string GetField(IDataRecord record, int recordIndex)
        {
            return innerColumn.GetField(record, recordIndex).ToString();
        }
    }
}