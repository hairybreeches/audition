using System.Web.Http;
using Audition.Chromium;
using Model.Responses;
using Model.SearchWindows;
using Persistence;
using Searching;
using Webapp.Requests;
using Webapp.Session;

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
            return Searcher.FindJournalsWithin(searchRequest.SearchWindow).GetPage(searchRequest.PageNumber);
        }        

        [HttpPost]
        [Route(Routing.AccountsSearch)]
        public SearchResponse AccountsSearch(SearchRequest<UnusualAccountsParameters> searchRequest)
        {
            return Searcher.FindJournalsWithin(searchRequest.SearchWindow).GetPage(searchRequest.PageNumber);
        }
        
        [HttpPost]
        [Route(Routing.DateSearch)]
        public SearchResponse DateSearch(SearchRequest<YearEndParameters> searchRequest)
        {
            return Searcher.FindJournalsWithin(searchRequest.SearchWindow).GetPage(searchRequest.PageNumber);
        }

        [HttpPost]
        [Route(Routing.UserSearch)]
        public SearchResponse UserSearch(SearchRequest<UserParameters> searchRequest)
        {
            return Searcher.FindJournalsWithin(searchRequest.SearchWindow).GetPage(searchRequest.PageNumber);
        }
        
        [HttpPost]
        [Route(Routing.EndingSearch)]
        public SearchResponse EndingSearch(SearchRequest<EndingParameters> searchRequest)
        {
            return Searcher.FindJournalsWithin(searchRequest.SearchWindow).GetPage(searchRequest.PageNumber);
        }
    }
}
