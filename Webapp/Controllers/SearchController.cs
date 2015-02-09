﻿using System;
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
        
        private readonly LoginSession session;

        public SearchController(LoginSession session)
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

        private SearchResponse Search<T>(SearchRequest<T> searchRequest) where T : ISearchParameters
        {
            return searchRequest.SearchWindow.Execute(Searcher).GetPage(searchRequest.PageNumber);
        }
    }
}
