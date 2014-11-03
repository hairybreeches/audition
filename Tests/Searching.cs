using System;
using System.Collections.Generic;
using Autofac;
using Model.Accounting;
using Model.SearchWindows;
using NUnit.Framework;
using Persistence;
using Searching;

namespace Tests
{
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

        private static IEnumerable<Journal> ExecuteSearch(Func<JournalRepository, IEnumerable<Journal>> searchAction, Journal[] journalsInRepository)
        {
            CollectionAssert.IsNotEmpty(journalsInRepository, "Searching an empty repository is not a useful test");

            using (var lifetime = AutofacConfiguration.CreateDefaultContainerBuilder().BuildSearchable(journalsInRepository))
            using (var requestScope = lifetime.BeginRequestScope())
            {
                return searchAction(requestScope.Resolve<JournalRepository>());
            }
        }
    }
}