using System;
using System.Data;

namespace SqlImport.Schema
{
    public class NullDataReader<T> : ISqlDataReader<T>
    {
        public T GetField(IDataRecord record, int recordIndex)
        {
            return default(T);
        }
    }
}