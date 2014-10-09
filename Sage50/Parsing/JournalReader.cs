using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model.Accounting;

namespace Sage50.Parsing
{
    public class JournalReader
    {
        private readonly IDataReader reader;

        public JournalReader(IDataReader reader)
        {
            this.reader = reader;
        }

        public IEnumerable<Journal> GetJournals()
        {
            return JournalParsing.ReadJournals(GetLineRecords().Select(ConvertToLine));
        }


        private IEnumerable<IDataRecord> GetLineRecords()
        {            
            while (reader.Read())
            {
                yield return reader;
            }
        }

        private static SageJournalLine ConvertToLine(IDataRecord record)
        {
            return JournalLineParsing.CreateJournalLine(record);
        }    
    }
}