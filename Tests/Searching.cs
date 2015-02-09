using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Model.Accounting;
using Model.Responses;
using Native;
using NSubstitute;
using NUnit.Framework;
using Persistence;
using Searching;
using Searching.SearchWindows;
using Webapp.Controllers;
using Webapp.Requests;

namespace Tests
{

    public static class Searching
    {
        public static IEnumerable<Journal> ExecuteSearch(ISearchWindow searchWindow, params Journal[] journalsInRepository)
        {
            //todo: shouldn't have to use a filesystem here
            var repo = new TempFileJournalRepository(new FileSystem());
            repo.UpdateJournals(journalsInRepository);

            var searcher = JournalSearcherFactory.EverythingAvailable.CreateJournalSearcher(repo);
            return searchWindow.Execute(searcher);
        }

        public static IEnumerable<Journal> ExecuteSearch(ISearchRequest request, IEnumerable<Journal> journalsInRepository)
        {
            CollectionAssert.IsNotEmpty(journalsInRepository, "Searching an empty repository is not a useful test");

            using (var lifetime = AutofacConfiguration
                .CreateDefaultContainerBuilder()
                .WithNoLicensing()
                .BuildSearchable(journalsInRepository))
            {
                return lifetime.Resolve<SearchController>().Search(request).Journals.ToList();
            }
        }
    }
}