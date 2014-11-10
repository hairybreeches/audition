using System;
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
            var totalPages = Math.Ceiling((double) totalResults / Constants.Pagesize);

            if (pageNumber > totalPages || pageNumber < 1)
            {
                throw new InvalidPageNumberException("Cannot go to page " + pageNumber + ". There are " + totalPages + " pages.");
            }

            var firstResult = (Constants.Pagesize*(pageNumber - 1)) + 1;

            var numberOfResultsToSkip = (pageNumber - 1) * Constants.Pagesize;
            var journalsToReturn = listOfAllJournals.Skip(numberOfResultsToSkip).Take(Constants.Pagesize).ToList();
            var isNextPage = pageNumber < totalPages;
            var isPreviousPage = pageNumber > 1;
            return new SearchResponse(journalsToReturn, totalResults.ToString(),  isPreviousPage, isNextPage, firstResult);
        }

        public static IEnumerable<Journal> GetAllJournals(this IQueryable<Journal> journals)
        {
            return journals;
        }
    }
}