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

        public static SearchResponse GetPage(this IQueryable<Transaction> journals, int pageNumber)
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

        private static string TotalResults(IEnumerable<Transaction> resultsSet, int skipped)
        {
            var minimumResults = resultsSet.Count() + skipped;
            return minimumResults > MaxTotal? String.Format("more than {0}", MaxTotal) : minimumResults.ToString();
        }

        private static void ValidatePageNumber(int pageNumber, IEnumerable<Transaction> listOfAllJournals)
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

        private static void ValidatePageNumberNotTooBig(int pageNumber, IEnumerable<Transaction> resultsSet)
        {
            if (!resultsSet.Any() && pageNumber > 1)
            {
                throw new InvalidPageNumberException(String.Format("Page number too big: {0}", pageNumber));
            }            
        }

        public static IEnumerable<Transaction> GetAllJournals(this IQueryable<Transaction> journals)
        {
            return journals;
        }        
    }
}