using System.Data;
using System.Linq;

namespace SqlImport
{
    public class SqlDataReader
    {
        private readonly IDataReader innerReader;
        public int RowNumber { get; private set; }

        public SqlDataReader(IDataReader innerReader, int firstRowNumber = 0)
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

        public bool RowIsEmpty()
        {
            return Enumerable.Range(0, innerReader.FieldCount)
                .All(innerReader.IsDBNull);
        }
    }
}
