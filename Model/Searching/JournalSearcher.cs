using System.Collections.Generic;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;

namespace Model.Searching
{
    public class JournalSearcher : IJournalSearcher
    {
        private readonly IJournalSearcher<WorkingHoursParameters> hoursSearcher;
        private readonly IJournalSearcher<YearEndParameters> yearEndSearcher;
        private readonly IJournalSearcher<UnusualAccountsParameters> unusualAccountsSearcher;
        private readonly IJournalSearcher<EndingParameters> roundNumberSearcher;
        private readonly IJournalSearcher<UserParameters> userSearcher;

        public JournalSearcher(IJournalSearcher<WorkingHoursParameters> hoursSearcher, IJournalSearcher<YearEndParameters> yearEndSearcher, IJournalSearcher<UnusualAccountsParameters> unusualAccountsSearcher, IJournalSearcher<EndingParameters> roundNumberSearcher, IJournalSearcher<UserParameters> userSearcher)
        {
            this.hoursSearcher = hoursSearcher;
            this.yearEndSearcher = yearEndSearcher;
            this.unusualAccountsSearcher = unusualAccountsSearcher;
            this.roundNumberSearcher = roundNumberSearcher;
            this.userSearcher = userSearcher;
        }       
        
        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<YearEndParameters> searchWindow)
        {
            return yearEndSearcher.FindJournalsWithin(searchWindow);
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

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<WorkingHoursParameters> searchWindow)
        {
            return hoursSearcher.FindJournalsWithin(searchWindow);
        }
    }
}
