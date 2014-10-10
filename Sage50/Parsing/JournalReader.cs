using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model.Accounting;

namespace Sage50.Parsing
{
    public class JournalReader
    {
        private readonly IDataReader reader;
        private readonly JournalLineParser journalLineParser;

        public JournalReader(IDataReader reader, JournalLineParser journalLineParser)
        {
            this.reader = reader;
            this.journalLineParser = journalLineParser;
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

        private SageJournalLine ConvertToLine(IDataRecord record)
        {
            return journalLineParser.CreateJournalLine(record);
        }    
    }
}