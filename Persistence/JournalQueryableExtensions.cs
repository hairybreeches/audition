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

            var listOfAllJournals = journals.Skip(numberOfResultsToSkip).Take(MaxTotal + 1).ToList();

            ValidatePageNumber(pageNumber, listOfAllJournals);


            var minimumTotalResults = listOfAllJournals.Count() + numberOfResultsToSkip;            

            var firstResult = numberOfResultsToSkip + 1;

            var journalsToReturn = listOfAllJournals.Take(Pagesize).ToList();
            var isNextPage = listOfAllJournals.Count > Pagesize;
            var isPreviousPage = pageNumber > 1;
            return new SearchResponse(journalsToReturn, minimumTotalResults > MaxTotal? String.Format("more than {0}", MaxTotal) : minimumTotalResults.ToString(),  isPreviousPage, isNextPage, firstResult);
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

        private static void ValidatePageNumberNotTooBig(int pageNumber, IEnumerable<Journal> listOfAllJournals)
        {
            if (!listOfAllJournals.Any() && pageNumber > 1)
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