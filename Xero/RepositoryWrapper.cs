using System.Collections.Generic;
using System.Linq;
using XeroApi;
using XeroApi.Model;

namespace Xero
{
    public class RepositoryWrapper : IFullRepository
    {
        public IEnumerable<Journal> Journals { get; private set; }

        public RepositoryWrapper(IEnumerable<Journal> journals)
        {
            Journals = journals.ToList();
        }
    }
}
