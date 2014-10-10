using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using Model;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;
using Sage50.Parsing;

namespace Sage50
{
    public class Sage50JournalSearcher : IJournalSearcher
    {
        private readonly SageConnectionFactory connectionFactory;

        public Sage50JournalSearcher(SageConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<WorkingHours> searchWindow)
        {
            using (var connection = connectionFactory.OpenConnection())
            {
                var command = GetAllJournalsCommand(connection);
                var reader = new JournalReader(command.ExecuteReader());
                return reader.GetJournals().ToList();
            }
        }

        private static OdbcCommand GetAllJournalsCommand(OdbcConnection conn)
        {
            return new OdbcCommand("SELECT 'TRAN_NUMBER','USER_NAME','DATE','RECORD_CREATE_DATE','NOMINAL_CODE','AMOUNT','DETAILS' FROM AUDIT_JOURNAL", conn);
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