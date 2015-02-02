using System.Data;

namespace SqlImport.DataReaders
{
    public class ToStringDataReader : ISqlDataReader<string>
    {
        private readonly int index;

        public ToStringDataReader(int index)
        {
            this.index = index;
        }
        public string GetField(IDataRecord record, int recordIndex)
        {
            return record.GetValue(index).ToString();
        }
    }
}