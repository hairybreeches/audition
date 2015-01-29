using System.Data;
using SqlImport.Schema;

namespace ExcelImport
{
    public class RecordNumberIdColumn : ISchemaColumn<int>
    {
        public RecordNumberIdColumn()
        {
            FieldName = "Id";
        }

        public int GetField(IDataRecord record, int recordIndex)
        {
            return recordIndex;
        }

        public DataColumn ToDataColumn()
        {
            return new DataColumn(FieldName);
        }

        public int Index { get; private set; }

        public string FieldName { get; private set; }
    }
}
