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
            //perhaps we could save a few seconds here by using a mock repository which stores the journals in memory.
            //however, it's important that we have at least one test for each searcher 
            //that makes sure the way we parse the journals is compatible with the way the repository delivers them 
            //(eg no multiple enumeration of IEnumerables)
            //and these tests are not currently slow enough to warrant sacrificing assurance
            var repo = new TempFileJournalRepository(new FileSystem());
            repo.UpdateJournals(journalsInRepository);

            var searcher = JournalSearcherFactory.EverythingAvailable.CreateJournalSearcher();
            return searchWindow.Execute(searcher, repo);
        }        
    }
}