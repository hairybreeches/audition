using System.Data;
using System.Linq;

namespace SqlImport
{
    public static class DataRecordExtensions
    {
        public static bool RowIsEmpty(this IDataRecord dataRecord)
        {
            return Enumerable.Range(0, dataRecord.FieldCount)
                .All(dataRecord.IsDBNull);
        }
    }
}