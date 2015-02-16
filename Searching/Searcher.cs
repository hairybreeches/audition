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

        public IQueryable<Transaction> FindTransactionsWithin(YearEndParameters parameters, IQueryable<Transaction> transactions)
        {
            return yearEndSearcher.FindTransactionsWithin(parameters, transactions);
        }

        public IQueryable<Transaction> FindTransactionsWithin(EndingParameters parameters, IQueryable<Transaction> transactions)
        {
            return roundNumberSearcher.FindTransactionsWithin(parameters, transactions);
        }

        public IQueryable<Transaction> FindTransactionsWithin(UnusualAccountsParameters parameters, IQueryable<Transaction> transactions)
        {
            return unusualAccountsSearcher.FindTransactionsWithin(parameters, transactions);
        }

        public IQueryable<Transaction> FindTransactionsWithin(UserParameters parameters, IQueryable<Transaction> transactions)
        {
            return userSearcher.FindTransactionsWithin(parameters, transactions);
        }

        public IQueryable<Transaction> FindTransactionsWithin(WorkingHoursParameters parameters, IQueryable<Transaction> transactions)
        {
            return hoursSearcher.FindTransactionsWithin(parameters, transactions);
        }
    }
}
