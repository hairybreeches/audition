using System.Data;

namespace SqlImport
{
    public class DataReader
    {
        private readonly IDataReader innerReader;
        public int RowNumber { get; private set; }

        public DataReader(IDataReader innerReader, int firstRowNumber = 0)
        {
            this.innerReader = innerReader;
            //to get the first row, we will have to call Read, which will increment by one
            RowNumber = firstRowNumber - 1;
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
