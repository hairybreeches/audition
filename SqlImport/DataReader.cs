using System.Data;

namespace SqlImport
{
    public class DataReader
    {
        private readonly IDataReader innerReader;
        public int RowNumber { get; private set; }

        public DataReader(IDataReader innerReader, int rowNumber = 0)
        {
            this.innerReader = innerReader;
            RowNumber = rowNumber;
        }

        public bool Read()
        {
            RowNumber++;
            return innerReader.Read();            
        }

        public IDataRecord CurrentRecord()
        {
            return innerReader;
        }
    }
}
