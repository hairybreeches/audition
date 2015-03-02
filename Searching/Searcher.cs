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
        private readonly ISearcher<UnusualAccountsParameters> unusualAccountsSearcher;
        private readonly ISearcher<EndingParameters> roundNumberSearcher;
        private readonly ISearcher<UserParameters> userSearcher;
        private readonly ISearcher<DuplicatePaymentsParameters> duplicatesSearcher;

        public Searcher(ISearcher<UnusualAccountsParameters> unusualAccountsSearcher, ISearcher<EndingParameters> roundNumberSearcher, ISearcher<UserParameters> userSearcher, ISearcher<DuplicatePaymentsParameters> duplicatesSearcher)
        {
            this.unusualAccountsSearcher = unusualAccountsSearcher;
            this.roundNumberSearcher = roundNumberSearcher;
            this.userSearcher = userSearcher;
            this.duplicatesSearcher = duplicatesSearcher;
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

        public IQueryable<Transaction> FindTransactionsWithin(DuplicatePaymentsParameters parameters, IQueryable<Transaction> transactions)
        {
            return duplicatesSearcher.FindTransactionsWithin(parameters, transactions);
        }
    }
}
