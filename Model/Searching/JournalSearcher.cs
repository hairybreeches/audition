using System;
using System.Collections.Generic;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;

namespace Model.Searching
{
    public class JournalSearcher : IJournalSearcher
    {
        private readonly IJournalSearcher<WorkingHours> hoursSearcher;
        private readonly IJournalSearcher<YearEndParameters> yearEndSearcher;
        private readonly IJournalSearcher<UnusualAccountsParameters> unusualAccountsSearcher;
        private readonly IJournalSearcher<EndingParameters> roundNumberSearcher;
        private readonly IJournalSearcher<KeywordParameters> keywordSearcher;
        private readonly IJournalSearcher<UserParameters> userSearcher;

        public JournalSearcher(IJournalSearcher<WorkingHours> hoursSearcher, IJournalSearcher<YearEndParameters> yearEndSearcher, IJournalSearcher<UnusualAccountsParameters> unusualAccountsSearcher, IJournalSearcher<EndingParameters> roundNumberSearcher, IJournalSearcher<KeywordParameters> keywordSearcher, IJournalSearcher<UserParameters> userSearcher)
        {
            this.hoursSearcher = hoursSearcher;
            this.yearEndSearcher = yearEndSearcher;
            this.unusualAccountsSearcher = unusualAccountsSearcher;
            this.roundNumberSearcher = roundNumberSearcher;
            this.keywordSearcher = keywordSearcher;
            this.userSearcher = userSearcher;
        }

        public static IJournalSearcher XeroJournalSearcher(JournalRepository repository)
        {
            return new JournalSearcher(
                new WorkingHoursSearcher(repository), 
                new YearEndSearcher(repository), 
                new UnusualAccountsSearcher(repository), 
                new RoundNumberSearcher(repository),
                new NotSupportedSearcher<KeywordParameters>("Xero does not have the concept of descriptions"), 
                new NotSupportedSearcher<UserParameters>("Xero does not record who raises individual journals"));
        }
        
        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<YearEndParameters> searchWindow)
        {
            return yearEndSearcher.FindJournalsWithin(searchWindow);
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<KeywordParameters> searchWindow)
        {
            return keywordSearcher.FindJournalsWithin(searchWindow);
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<EndingParameters> searchWindow)
        {
            return roundNumberSearcher.FindJournalsWithin(searchWindow);
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<UnusualAccountsParameters> searchWindow)
        {
            return unusualAccountsSearcher.FindJournalsWithin(searchWindow);
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<UserParameters> searchWindow)
        {
            return userSearcher.FindJournalsWithin(searchWindow);
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<WorkingHours> searchWindow)
        {
            return hoursSearcher.FindJournalsWithin(searchWindow);
        }
    }
}
