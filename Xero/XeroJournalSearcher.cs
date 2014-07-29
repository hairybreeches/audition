using System.Collections.Generic;
using System.Linq;
using Model;
using XeroApi.Model;

namespace Xero
{
    public class XeroJournalSearcher : IJournalSearcher
    {
        private readonly IRepositoryFactory repositoryFactory;

        public XeroJournalSearcher(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public IEnumerable<Model.Journal> FindJournalsWithin(TimeFrame timeFrame)
        {
            var repository = repositoryFactory.CreateRepository();
            return repository.Journals.ToList().Select(x => x.ToModelJournal());
        }
    }
}
