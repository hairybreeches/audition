using System.Data;

namespace Sage50
{
    public interface ISchemaColumn
    {
        DataColumn ToDataColumn();
        int Index { get; }
        string FieldName { get; }
    }
}