using System.Data;

namespace SqlImport.DataReaders
{
    public class NullDataReader<T> : IFieldReader<T>
    {
        public T GetField(IDataRecord record, int recordIndex)
        {
            return default(T);
        }
    }
}