using System.Data;

namespace SqlImport.DataReaders
{
    public class RecordNumberReader : IFieldReader<string>
    {        
        public string GetField(IDataRecord record, int recordIndex)
        {
            return recordIndex.ToString();
        }
    }
}
