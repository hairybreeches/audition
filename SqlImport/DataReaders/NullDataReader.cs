using System.Data;

namespace SqlImport.DataReaders
{
    public class NullDataReader<T> : ISqlDataReader<T>
    {
        public T GetField(IDataRecord record, int recordIndex)
        {
            return default(T);
        }
    }
}