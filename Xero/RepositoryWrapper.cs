using System.Linq;
using XeroApi;
using XeroApi.Model;

namespace Xero
{
    class RepositoryWrapper : IFullRepository
    {
        private readonly Repository repository;

        public RepositoryWrapper(Repository repository)
        {
            this.repository = repository;
        }

        public IQueryable<Journal> Journals
        {
            get { return repository.Journals; }
        }
    }
}
