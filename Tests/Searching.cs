using System;
using System.Collections.Generic;
using System.Linq;
using Audition.Controllers;
using Autofac;
using Model.Accounting;
using Model.Responses;
using Model.SearchWindows;
using NUnit.Framework;
using Persistence;
using Searching;
using Webapp.Requests;

namespace Tests
{
    //todo: so much duplication!
    public static class Searching
    {
        public static IEnumerable<Journal> ExecuteSearch(SearchWindow<WorkingHoursParameters> searchWindow, params Journal[] journalsInRepository)
        {
            return ExecuteSearch(repo => new WorkingHoursSearcher(repo).FindJournalsWithin(searchWindow), journalsInRepository);
        }     
        
        public static IEnumerable<Journal> ExecuteSearch(SearchWindow<UnusualAccountsParameters> searchWindow, params Journal[] journalsInRepository)
        {
            return ExecuteSearch(repo => new UnusualAccountsSearcher(repo).FindJournalsWithin(searchWindow), journalsInRepository);
        }
        
        public static IEnumerable<Journal> ExecuteSearch(SearchWindow<YearEndParameters> searchWindow, params Journal[] journalsInRepository)
        {
            return ExecuteSearch(repo => new YearEndSearcher(repo).FindJournalsWithin(searchWindow), journalsInRepository);
        }     
        
        public static IEnumerable<Journal> ExecuteSearch(SearchWindow<UserParameters> searchWindow, params Journal[] journalsInRepository)
        {
            return ExecuteSearch(repo => new UserSearcher(repo).FindJournalsWithin(searchWindow), journalsInRepository);
        }     
        
        public static IEnumerable<Journal> ExecuteSearch(SearchWindow<EndingParameters> searchWindow, params Journal[] journalsInRepository)
        {
            return ExecuteSearch(repo => new RoundNumberSearcher(repo).FindJournalsWithin(searchWindow), journalsInRepository);
        }

        public static IEnumerable<Journal> ExecuteSearch(SearchRequest<WorkingHoursParameters> searchRequest, IEnumerable<Journal> journals)
        {
            return ExecuteSearch(controller => controller.HoursSearch(searchRequest), journals);
        }

        public static IEnumerable<Journal> ExecuteSearch(SearchRequest<UnusualAccountsParameters> searchRequest, IEnumerable<Journal> journals)
        {
            return ExecuteSearch(controller => controller.AccountsSearch(searchRequest), journals);
        }

        public static IEnumerable<Journal> ExecuteSearch(SearchRequest<YearEndParameters> searchRequest, IEnumerable<Journal> journals)
        {
            return ExecuteSearch(controller => controller.DateSearch(searchRequest), journals);
        }

        public static IEnumerable<Journal> ExecuteSearch(SearchRequest<UserParameters> searchRequest, IEnumerable<Journal> journals)
        {
            return ExecuteSearch(controller => controller.UserSearch(searchRequest), journals);
        }

        public static IEnumerable<Journal> ExecuteSearch(SearchRequest<EndingParameters> searchRequest, IEnumerable<Journal> journals)
        {
            return ExecuteSearch(controller => controller.EndingSearch(searchRequest), journals);
        }

        private static IEnumerable<Journal> ExecuteSearch(Func<SearchController, SearchResponse> searchAction, IEnumerable<Journal> journalsInRepository)
        {
            return ExecuteSearch(context => searchAction(context.Resolve<SearchController>()).Journals, journalsInRepository);
        }

        private static IEnumerable<Journal> ExecuteSearch(Func<IJournalRepository, IEnumerable<Journal>> searchAction, IEnumerable<Journal> journalsInRepository)
        {
            return ExecuteSearch(context => searchAction(context.Resolve<IJournalRepository>()), journalsInRepository);
        }

        private static IEnumerable<Journal> ExecuteSearch(Func<IComponentContext, IEnumerable<Journal>> searchAction, IEnumerable<Journal> journalsInRepository)
        {
            CollectionAssert.IsNotEmpty(journalsInRepository, "Searching an empty repository is not a useful test");

            using (var lifetime = AutofacConfiguration.CreateDefaultContainerBuilder().BuildSearchable(journalsInRepository))
            using (var requestScope = lifetime.BeginRequestScope())
            {
                return searchAction(requestScope).ToList();
            }
        }

        
    }
}