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
    }
}