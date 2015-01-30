using System.Data;

namespace SqlImport.Schema
{
    public interface ISqlDataReader<out T>
    {
        T GetField(IDataRecord record, int recordIndex);
    }
}