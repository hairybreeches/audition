using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using Model;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;
using Sage50.Parsing;
using Sage50.Parsing.Schema;

namespace Sage50
{
    public class Sage50JournalSearcher : IJournalSearcher
    {
        private readonly SageConnectionFactory connectionFactory;
        private readonly JournalSchema schema;
        private readonly JournalReader journalReader;

        public Sage50JournalSearcher(SageConnectionFactory connectionFactory, JournalSchema schema,JournalReader journalReader)
        {
            this.connectionFactory = connectionFactory;
            this.schema = schema;
            this.journalReader = journalReader;
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<WorkingHours> searchWindow)
        {
            using (var connection = connectionFactory.OpenConnection())
            {
                var command = GetAllJournalsCommand(connection);                
                return journalReader.GetJournals(command.ExecuteReader()).ToList();
            }
        }

        private OdbcCommand GetAllJournalsCommand(OdbcConnection conn)
        {
            return new OdbcCommand(String.Format("SELECT {0} FROM AUDIT_JOURNAL", String.Join(",", schema.ColumnNames)), conn);
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<UnusualAccountsParameters> searchWindow)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<YearEndParameters> searchWindow)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<UserParameters> searchWindow)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<KeywordParameters> searchWindow)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<EndingParameters> searchWindow)
        {
            throw new NotImplementedException();
        }
    }
}