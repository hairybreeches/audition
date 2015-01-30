using System.Data;

namespace SqlImport.Schema
{
    public interface ISchemaColumn<out T> : ISchemaColumn
    {
        T GetField(IDataRecord record, int recordIndex);
    }

    public interface ISchemaColumn
    {
        DataColumn ToDataColumn();
        int Index { get; }
        string FieldName { get; }
    }
}