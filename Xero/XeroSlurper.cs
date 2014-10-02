using System.Collections.Generic;
using System.Linq;
using XeroApi;
using XeroApi.Model;

namespace Xero
{
    public class XeroSlurper
    {
        public IEnumerable<Journal> Slurp(Repository repository)
        {
            
            for (int i = 0; i < 30; i++)
            {
                var journals = repository.Journals.Skip(100*i).ToList();

                foreach (var journal in journals)
                {
                    yield return journal;
                }

                if (!journals.Any() || journals.Count < 100)
                    break;
            }
        }
    }
}
