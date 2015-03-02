using System.Collections.Generic;
using System.Linq;
using Capabilities;
using Model.Accounting;
using Native;
using Native.Disk;
using Persistence;
using Searching;

namespace Tests
{

    public static class Searching
    {
        public static IEnumerable<Transaction> ExecuteSearch(ISearchWindow searchWindow, params Transaction[] transactionsInRepository)
        {
            //perhaps we could save a few seconds here by using a mock repository which stores the transactions in memory.
            //however, it's important that we have at least one test for each searcher 
            //that makes sure the way we parse the transactions is compatible with the way the repository delivers them 
            //(eg no multiple enumeration of IEnumerables)
            //and these tests are not currently slow enough to warrant sacrificing assurance
            var repo = new TempFileTransactionRepository(new FileSystem());
            repo.UpdateTransactions(transactionsInRepository);

            var searcher = new SearcherFactory(Enumerable.Empty<SearchAction>(), new DisplayFieldProvider().AllFields.ToArray()).CreateSearcher();
            return searchWindow.Execute(searcher, repo);
        }        
    }
}