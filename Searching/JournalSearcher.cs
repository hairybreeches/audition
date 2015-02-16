using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;
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

        public IQueryable<Transaction> FindJournalsWithin(YearEndParameters parameters, IQueryable<Transaction> journals)
        {
            return yearEndSearcher.FindJournalsWithin(parameters, journals);
        }

        public IQueryable<Transaction> FindJournalsWithin(EndingParameters parameters, IQueryable<Transaction> journals)
        {
            return roundNumberSearcher.FindJournalsWithin(parameters, journals);
        }

        public IQueryable<Transaction> FindJournalsWithin(UnusualAccountsParameters parameters, IQueryable<Transaction> journals)
        {
            return unusualAccountsSearcher.FindJournalsWithin(parameters, journals);
        }

        public IQueryable<Transaction> FindJournalsWithin(UserParameters parameters, IQueryable<Transaction> journals)
        {
            return userSearcher.FindJournalsWithin(parameters, journals);
        }

        public IQueryable<Transaction> FindJournalsWithin(WorkingHoursParameters parameters, IQueryable<Transaction> journals)
        {
            return hoursSearcher.FindJournalsWithin(parameters, journals);
        }
    }
}
