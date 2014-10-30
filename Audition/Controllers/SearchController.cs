using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Audition.Chromium;
using Audition.Requests;
using Audition.Responses;
using Audition.Session;
using Model;
using Model.Accounting;
using Model.SearchWindows;
using Searching;

namespace Audition.Controllers
{
    public class SearchController : ApiController
    {
        private JournalSearcher Searcher
        {
            get { return session.GetCurrentJournalSearcher(); }
        }
        
        private readonly LoginSession session;

        public SearchController(LoginSession session)
        {
            this.session = session;
        }

        [HttpPost]
        [Route(Routing.HoursSearch)]
        public SearchResponse HoursSearch(SearchRequest<WorkingHoursParameters> searchRequest)
        {           
            //todo: duplication!
            var journals = Searcher.FindJournalsWithin(searchRequest.SearchWindow);
            return SearchResults(journals, searchRequest.PageNumber);
        }        

        [HttpPost]
        [Route(Routing.AccountsSearch)]
        public SearchResponse AccountsSearch(SearchRequest<UnusualAccountsParameters> searchRequest)
        {
            var journals = Searcher.FindJournalsWithin(searchRequest.SearchWindow);
            return SearchResults(journals, searchRequest.PageNumber);
        }
        
        [HttpPost]
        [Route(Routing.DateSearch)]
        public SearchResponse DateSearch(SearchRequest<YearEndParameters> searchRequest)
        {
            var journals = Searcher.FindJournalsWithin(searchRequest.SearchWindow);
            return SearchResults(journals, searchRequest.PageNumber);
        }

        [HttpPost]
        [Route(Routing.UserSearch)]
        public SearchResponse UserSearch(SearchRequest<UserParameters> searchRequest)
        {
            var journals = Searcher.FindJournalsWithin(searchRequest.SearchWindow);
            return SearchResults(journals, searchRequest.PageNumber);
        }
        
        [HttpPost]
        [Route(Routing.EndingSearch)]
        public SearchResponse EndingSearch(SearchRequest<EndingParameters> searchRequest)
        {
            var journals = Searcher.FindJournalsWithin(searchRequest.SearchWindow);
            return SearchResults(journals, searchRequest.PageNumber);
        }

        private static SearchResponse SearchResults(IEnumerable<Journal> journals, int pageNumber)
        {
            //todo: tests to make sure page number is treated correctly
            var listOfAllJournals = journals.ToList();
            var totalResults = listOfAllJournals.Count();
            var numberOfResultsToSkip = (pageNumber - 1) * Constants.Pagesize;
            var journalsToReturn = listOfAllJournals.Skip(numberOfResultsToSkip).Take(Constants.Pagesize);
            return new SearchResponse(journalsToReturn, totalResults);
        }
    }
}
