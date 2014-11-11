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
        private const int MaxTotal = 2000;
        private const int Pagesize = 10;

        public static SearchResponse GetPage(this IQueryable<Journal> journals, int pageNumber)
        {
            var numberOfResultsToSkip = (pageNumber - 1) * Pagesize;

            var resultsSet = journals.Skip(numberOfResultsToSkip).Take(MaxTotal + 1).ToList();

            ValidatePageNumber(pageNumber, resultsSet);


            var firstResult = numberOfResultsToSkip + 1;

            var journalsToReturn = resultsSet.Take(Pagesize).ToList();
            var isNextPage = resultsSet.Count > Pagesize;
            var isPreviousPage = pageNumber > 1;

            var totalResults = TotalResults(resultsSet, numberOfResultsToSkip);

            return new SearchResponse(journalsToReturn, totalResults,  isPreviousPage, isNextPage, firstResult);
        }

        private static string TotalResults(IEnumerable<Journal> resultsSet, int skipped)
        {
            var minimumResults = resultsSet.Count() + skipped;
            return minimumResults > MaxTotal? String.Format("more than {0}", MaxTotal) : minimumResults.ToString();
        }

        private static void ValidatePageNumber(int pageNumber, IEnumerable<Journal> listOfAllJournals)
        {
            ValidatePageNumberNotTooBig(pageNumber, listOfAllJournals);
            ValidatePageNumberNotTooSmall(pageNumber);
        }

        private static void ValidatePageNumberNotTooSmall(int pageNumber)
        {
            if (pageNumber < 1)
            {
                throw new InvalidPageNumberException(String.Format("Page number {0} too small", pageNumber));
            }
        }

        private static void ValidatePageNumberNotTooBig(int pageNumber, IEnumerable<Journal> allJournals)
        {
            if (!allJournals.Any() && pageNumber > 1)
            {
                throw new InvalidPageNumberException(String.Format("Page number too big: {0}", pageNumber));
            }            
        }

        public static IEnumerable<Journal> GetAllJournals(this IQueryable<Journal> journals)
        {
            return journals;
        }        
    }
}