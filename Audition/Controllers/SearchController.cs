using System.Collections.Generic;
using System.Web.Http;
using Audition.Chromium;
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
        public IEnumerable<Journal> HoursSearch(SearchWindow<WorkingHours> searchWindow)
        {
            return Searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route(Routing.AccountsSearch)]
        public IEnumerable<Journal> AccountsSearch(SearchWindow<UnusualAccountsParameters> searchWindow)
        {
            return Searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route(Routing.DateSearch)]
        public IEnumerable<Journal> DateSearch(SearchWindow<YearEndParameters> searchWindow)
        {
            return Searcher.FindJournalsWithin(searchWindow);
        }

        [HttpPost]
        [Route(Routing.UserSearch)]
        public IEnumerable<Journal> UserSearch(SearchWindow<UserParameters> searchWindow)
        {
            return Searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route(Routing.EndingSearch)]
        public IEnumerable<Journal> EndingSearch(SearchWindow<EndingParameters> searchWindow)
        {
            return Searcher.FindJournalsWithin(searchWindow);
        }
    }
}
