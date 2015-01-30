using System.Data;

namespace SqlImport.DataReaders
{
    public interface ISqlDataReader<out T>
    {
        T GetField(IDataRecord record, int recordIndex);
    }
}