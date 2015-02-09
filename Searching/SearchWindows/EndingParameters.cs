using System;
using System.Linq;
using Model.Accounting;
using Model.Time;

namespace Searching.SearchWindows
{
    public class EndingParameters : ISearchParameters
    {
        public int MinimumZeroesToBeConsideredUnusual { get; private set; }

        public EndingParameters(int minimumZeroesToBeConsideredUnusual)
        {
            MinimumZeroesToBeConsideredUnusual = minimumZeroesToBeConsideredUnusual;
        }

        public int Magnitude()
        {
            return (int) Math.Pow(10, MinimumZeroesToBeConsideredUnusual);
        }

        public override string ToString()
        {
            return String.Format("Ending in at least {0} zeroes", MinimumZeroesToBeConsideredUnusual);
        }

        public Func<DateRange, IQueryable<Journal>> GetSearchMethod(JournalSearcher searcher)
        {
            return dateRange => searcher.FindJournalsWithin(this, dateRange);
        }
    }
}