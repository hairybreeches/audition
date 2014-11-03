using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Accounting;
using Model.Responses;

namespace Persistence
{
    public static class JournalQueryableExtensions
    {
        public static SearchResponse GetPage(this IQueryable<Journal> journals, int pageNumber)
        {
            var listOfAllJournals = journals.ToList();
            var totalResults = listOfAllJournals.Count();
            var numberOfResultsToSkip = (pageNumber - 1) * Constants.Pagesize;
            var journalsToReturn = listOfAllJournals.OrderBy(x=>x.Created).Skip(numberOfResultsToSkip).Take(Constants.Pagesize).ToList();
            return new SearchResponse(journalsToReturn, totalResults);
        }

        public static IEnumerable<Journal> GetAllJournals(this IQueryable<Journal> journals)
        {
            return journals;
        }
    }
}