using System;
using System.Collections.Generic;
using Model;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;

namespace Sage50
{
    public class JournalSearcher : IJournalSearcher
    {
        private readonly SageConnectionFactory loginDetails;

        public JournalSearcher(SageConnectionFactory loginDetails)
        {
            this.loginDetails = loginDetails;
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<WorkingHours> searchWindow)
        {
            throw new NotImplementedException();
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