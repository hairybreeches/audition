using System.Collections.Generic;
using System.Linq;
using Model;

namespace Xero
{
    class XeroJournalSearcher : IJournalSearcher
    {
        private readonly IRepositoryFactory repositoryFactory;

        public XeroJournalSearcher(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public IEnumerable<string> FindJournalsWithin(TimeFrame timeFrame)
        {
            var repository = repositoryFactory.CreateRepository();
            return repository.Journals.Select(x => x.ToString());
        }
    }
}
