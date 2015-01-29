using System.Data;

namespace SqlImport
{
    public interface ISchemaColumn
    {
        DataColumn ToDataColumn();
        int Index { get; }
        string FieldName { get; }
    }
    
    public interface ISchemaColumn<T> : ISchemaColumn
    {
        T GetField(IDataRecord record);
    }
}