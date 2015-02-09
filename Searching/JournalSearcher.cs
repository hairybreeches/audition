using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Searching.SearchWindows;

namespace Searching
{
    public class JournalSearcher
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

        public IQueryable<Journal> FindJournalsWithin(YearEndParameters parameters, DateRange dateRange)
        {
            return yearEndSearcher.FindJournalsWithin(parameters, dateRange);
        }

        public IQueryable<Journal> FindJournalsWithin(EndingParameters parameters, DateRange dateRange)
        {
            return roundNumberSearcher.FindJournalsWithin(parameters, dateRange);
        }

        public IQueryable<Journal> FindJournalsWithin(UnusualAccountsParameters parameters, DateRange dateRange)
        {
            return unusualAccountsSearcher.FindJournalsWithin(parameters, dateRange);
        }

        public IQueryable<Journal> FindJournalsWithin(UserParameters parameters, DateRange dateRange)
        {
            return userSearcher.FindJournalsWithin(parameters, dateRange);
        }

        public IQueryable<Journal> FindJournalsWithin(WorkingHoursParameters parameters, DateRange dateRange)
        {
            return hoursSearcher.FindJournalsWithin(parameters, dateRange);
        }
    }
}
