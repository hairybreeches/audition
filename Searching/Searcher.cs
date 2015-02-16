using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public class Searcher
    {
        private readonly ISearcher<WorkingHoursParameters> hoursSearcher;
        private readonly ISearcher<YearEndParameters> yearEndSearcher;
        private readonly ISearcher<UnusualAccountsParameters> unusualAccountsSearcher;
        private readonly ISearcher<EndingParameters> roundNumberSearcher;
        private readonly ISearcher<UserParameters> userSearcher;

        public Searcher(ISearcher<WorkingHoursParameters> hoursSearcher, ISearcher<YearEndParameters> yearEndSearcher, ISearcher<UnusualAccountsParameters> unusualAccountsSearcher, ISearcher<EndingParameters> roundNumberSearcher, ISearcher<UserParameters> userSearcher)
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
