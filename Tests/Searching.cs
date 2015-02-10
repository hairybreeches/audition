using System.Collections.Generic;
using Model.Accounting;
using Native;
using Persistence;
using Searching;

namespace Tests
{

    public static class Searching
    {
        public static IEnumerable<Journal> ExecuteSearch(ISearchWindow searchWindow, params Journal[] journalsInRepository)
        {
            //todo: shouldn't have to use a filesystem here
            var repo = new TempFileJournalRepository(new FileSystem());
            repo.UpdateJournals(journalsInRepository);

            var searcher = JournalSearcherFactory.EverythingAvailable.CreateJournalSearcher();
            return searchWindow.Execute(searcher, repo);
        }        
    }
}