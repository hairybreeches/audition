using System.Data;

namespace SqlImport.DataReaders
{
    public interface IFieldReader<out T>
    {
        T GetField(IDataRecord record, int recordIndex);
    }
}