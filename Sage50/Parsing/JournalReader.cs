using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model.Accounting;

namespace Sage50.Parsing
{
    public class JournalReader
    {
        private readonly JournalLineParser journalLineParser;

        public JournalReader(JournalLineParser journalLineParser)
        {
            this.journalLineParser = journalLineParser;
        }

        public IEnumerable<Journal> GetJournals(IDataReader reader)
        {
            return JournalParsing.ReadJournals(GetLineRecords(reader).Select(ConvertToLine));
        }


        private IEnumerable<IDataRecord> GetLineRecords(IDataReader reader)
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