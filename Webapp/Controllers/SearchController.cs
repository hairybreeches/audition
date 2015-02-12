using System;
using System.Linq;
using System.Web.Http;
using Model.Accounting;
using Model.Responses;
using Persistence;
using Searching;
using Searching.SearchWindows;
using Webapp.Requests;
using Webapp.Session;

namespace Webapp.Controllers
{
    public class SearchController : ApiController
    {
        private JournalSearcher Searcher
        {
            get { return session.GetCurrentJournalSearcher(); }
        }

        private IJournalRepository Repository
        {
            get { return session.Repository; }
        }
        
        private readonly Session.Session session;

        public SearchController(Session.Session session)
        {
            this.session = session;
        }

        [HttpPost]
        [Route(Routing.HoursSearch)]
        public SearchResponse HoursSearch(SearchRequest<WorkingHoursParameters> searchRequest)
        {
            return Search(searchRequest);
        }        

        [HttpPost]
        [Route(Routing.AccountsSearch)]
        public SearchResponse AccountsSearch(SearchRequest<UnusualAccountsParameters> searchRequest)
        {
            return Search(searchRequest);
        }
        
        [HttpPost]
        [Route(Routing.DateSearch)]
        public SearchResponse DateSearch(SearchRequest<YearEndParameters> searchRequest)
        {
            return Search(searchRequest);
        }

        [HttpPost]
        [Route(Routing.UserSearch)]
        public SearchResponse UserSearch(SearchRequest<UserParameters> searchRequest)
        {
            return Search(searchRequest);
        }
        
        [HttpPost]
        [Route(Routing.EndingSearch)]
        public SearchResponse EndingSearch(SearchRequest<EndingParameters> searchRequest)
        {
            return Search(searchRequest);
        }

        public SearchResponse Search(ISearchRequest searchRequest)
        {
            return searchRequest.SearchWindow.Execute(Searcher, Repository).GetPage(searchRequest.PageNumber);
        }
    }
}
