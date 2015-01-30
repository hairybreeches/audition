using System.Data;

namespace SqlImport.Schema
{
    public class RecordNumberIdColumn : ISchemaColumn<string>
    {
        public RecordNumberIdColumn()
        {
            FieldName = "Id";
            Index = -1;
        }

        public string GetField(IDataRecord record, int recordIndex)
        {
            return recordIndex.ToString();
        }

        public DataColumn ToDataColumn()
        {
            return new DataColumn(FieldName);
        }

        public int Index { get; private set; }

        public string FieldName { get; private set; }
    }
}
