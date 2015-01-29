﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model.Accounting;
using SqlImport;

namespace Sage50.Parsing
{
    public class JournalReader
    {
        private readonly JournalLineParser journalLineParser;

        public JournalReader(JournalLineParser journalLineParser)
        {
            this.journalLineParser = journalLineParser;
        }

        public IEnumerable<Journal> GetJournals(IDataReader reader, NominalCodeLookup nominalCodeLookup)
        {
            return JournalParsing.ReadJournals(GetLineRecords(reader).Select(record => ConvertToLine(record, nominalCodeLookup)));
        }


        private IEnumerable<IDataRecord> GetLineRecords(IDataReader reader)
        {            
            while (reader.Read())
            {
                yield return reader;
            }
        }

        private SqlJournalLine ConvertToLine(IDataRecord record, NominalCodeLookup lookup)
        {
            var sqlJournalLine = journalLineParser.CreateJournalLine(record);
            sqlJournalLine.NominalCodeName = lookup.GetNominalCodeName(sqlJournalLine.NominalCode);
            return sqlJournalLine;
        }
    }
}