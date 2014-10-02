using System.Collections.Generic;
using System.Linq;
using XeroApi;
using XeroApi.Model;

namespace Xero
{
    class RepositoryWrapper : IFullRepository
    {
        public IEnumerable<Journal> Journals { get; set; }

        public RepositoryWrapper(IEnumerable<Journal> journals)
        {
            Journals = journals;
        }
    }
}
