using System.Data;

namespace SqlImport.DataReaders
{
    public class FieldReader : IFieldReader<object>
    {
        private readonly int index;

        public FieldReader(int index)
        {
            this.index = index;
        }

        public object GetField(IDataRecord record, int recordIndex)
        {
            return record[index];            
        }
    }
}