using System.Data;

namespace SqlImport.Schema
{
    public class RecordNumberReader : ISqlDataReader<string>
    {        
        public string GetField(IDataRecord record, int recordIndex)
        {
            return recordIndex.ToString();
        }
    }
}
