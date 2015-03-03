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
        private readonly ISearcher<UnusualNominalCodesParameters> unusualNominalCodesSearcher;
        private readonly ISearcher<EndingParameters> roundNumberSearcher;
        private readonly ISearcher<UserParameters> userSearcher;

        public Searcher(ISearcher<UnusualNominalCodesParameters> unusualNominalCodesSearcher, ISearcher<EndingParameters> roundNumberSearcher, ISearcher<UserParameters> userSearcher)
        {
            this.unusualNominalCodesSearcher = unusualNominalCodesSearcher;
            this.roundNumberSearcher = roundNumberSearcher;
            this.userSearcher = userSearcher;
        }

        public IQueryable<Transaction> FindTransactionsWithin(EndingParameters parameters, IQueryable<Transaction> transactions)
        {
            return roundNumberSearcher.FindTransactionsWithin(parameters, transactions);
        }

        public IQueryable<Transaction> FindTransactionsWithin(UnusualNominalCodesParameters parameters, IQueryable<Transaction> transactions)
        {
            return unusualNominalCodesSearcher.FindTransactionsWithin(parameters, transactions);
        }

        public IQueryable<Transaction> FindTransactionsWithin(UserParameters parameters, IQueryable<Transaction> transactions)
        {
            return userSearcher.FindTransactionsWithin(parameters, transactions);
        }
    }
}
