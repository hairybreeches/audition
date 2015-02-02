using System.Data;
using Model;

namespace SqlImport.DataReaders
{
    public class LookupConverter<TIn, TOut> : ISqlDataReader<TOut>
    {
        private readonly ISqlDataReader<TIn> innerReader;
        private readonly IValueLookup<TIn, TOut> lookup;

        public LookupConverter(ISqlDataReader<TIn> innerReader, IValueLookup<TIn, TOut> lookup)
        {
            this.innerReader = innerReader;
            this.lookup = lookup;
        }

        public TOut GetField(IDataRecord record, int recordIndex)
        {
            return lookup.GetLookupValue(innerReader.GetField(record, recordIndex));
        }
    }
}
