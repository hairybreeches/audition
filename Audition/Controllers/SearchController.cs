using System.Collections.Generic;
using System.Web.Http;
using Audition.Chromium;
using Audition.Requests;
using Audition.Session;
using Model.Accounting;
using Model.Searching;
using Model.SearchWindows;
using Model.Time;

namespace Audition.Controllers
{
    public class SearchController : ApiController
    {
        private IJournalSearcher Searcher
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
        public IEnumerable<Journal> HoursSearch(SearchRequest<WorkingHours> searchRequest)
        {
            return Searcher.FindJournalsWithin(searchRequest.SearchWindow);
        }
        
        [HttpPost]
        [Route(Routing.AccountsSearch)]
        public IEnumerable<Journal> AccountsSearch(SearchRequest<UnusualAccountsParameters> searchRequest)
        {
            return Searcher.FindJournalsWithin(searchRequest.SearchWindow);
        }
        
        [HttpPost]
        [Route(Routing.DateSearch)]
        public IEnumerable<Journal> DateSearch(SearchRequest<YearEndParameters> searchRequest)
        {
            return Searcher.FindJournalsWithin(searchRequest.SearchWindow);
        }

        [HttpPost]
        [Route(Routing.UserSearch)]
        public IEnumerable<Journal> UserSearch(SearchRequest<UserParameters> searchRequest)
        {
            return Searcher.FindJournalsWithin(searchRequest.SearchWindow);
        }
        
        [HttpPost]
        [Route(Routing.EndingSearch)]
        public IEnumerable<Journal> EndingSearch(SearchRequest<EndingParameters> searchRequest)
        {
            return Searcher.FindJournalsWithin(searchRequest.SearchWindow);
        }
    }
}
