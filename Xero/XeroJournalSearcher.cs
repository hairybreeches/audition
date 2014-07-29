using System.Collections.Generic;
using System.Linq;
using Model;
using XeroApi.Model;

namespace Xero
{
    public class XeroJournalSearcher : IJournalSearcher
    {
        private readonly IFullRepository repository;

        public XeroJournalSearcher(IRepositoryFactory repositoryFactory)
        {
            repository = repositoryFactory.CreateRepository();
        }

        public IEnumerable<Model.Journal> FindJournalsWithin(TimeFrame timeFrame)
        {            
            return repository.Journals.ToList().Select(x => x.ToModelJournal());
        }
    }
}
