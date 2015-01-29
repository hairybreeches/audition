using System.Data;

namespace SqlImport.Schema
{
    public interface ISchemaColumn<T> : ISchemaColumn
    {
        T GetField(IDataRecord record);
    }

    public interface ISchemaColumn
    {
        DataColumn ToDataColumn();
        int Index { get; }
        string FieldName { get; }
    }
}