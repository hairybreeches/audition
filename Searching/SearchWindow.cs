
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{    
    public class SearchWindow<T> : ISearchWindow where T: ISearchParameters
    {
        public SearchWindow(T parameters, DateRange period)
        {
            Period = period;
            Parameters = parameters;
        }

        public T Parameters { get; private set; }
        public DateRange Period { get; private set; }

        public string Description
        {
            get { return ToString(); }
        }

        public override string ToString()
        {
            return string.Format("Journals {0}, in the period {1}", Parameters, Period);
        }

        protected bool Equals(SearchWindow<T> other)
        {
            return Equals(Parameters, other.Parameters) && Equals(Period, other.Period);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SearchWindow<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Parameters != null ? Parameters.GetHashCode() : 0)*397) ^ (Period != null ? Period.GetHashCode() : 0);
            }
        }

        public IQueryable<Transaction> Execute(Searcher searcher, ITransactionRepository repository)
        {
            return Parameters.ApplyFilter(searcher, GetJournalsApplyingToPeriod(repository));
        }

        private IQueryable<Transaction> GetJournalsApplyingToPeriod(ITransactionRepository repository)
        {
            return repository.GetTransactions().Where(x => Period.Contains(x.JournalDate));
        }
    }
}