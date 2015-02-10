using System.Data;

namespace Sage50.Parsing
{
    public interface ISchemaColumn
    {
        DataColumn ToDataColumn();
        int Index { get; }
        string FieldName { get; }
    }
}