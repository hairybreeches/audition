using System.Linq;
using XeroApi;
using XeroApi.Model;

namespace Xero
{
    public class XeroJournalSource : IXeroJournalSource
    {
        private readonly Repository repository;

        public XeroJournalSource(Repository repository)
        {
            this.repository = repository;
        }

        public IQueryable<Journal> Journals
        {
            get { return repository.Journals; }
        }
    }
}