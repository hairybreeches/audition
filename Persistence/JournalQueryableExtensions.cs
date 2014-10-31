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
            //todo: tests to make sure page number is treated correctly
            var listOfAllJournals = journals.ToList();
            var totalResults = listOfAllJournals.Count();
            var numberOfResultsToSkip = (pageNumber - 1) * Constants.Pagesize;
            var journalsToReturn = listOfAllJournals.Skip(numberOfResultsToSkip).Take(Constants.Pagesize).ToList();
            return new SearchResponse(journalsToReturn, totalResults);
        }
    }
}