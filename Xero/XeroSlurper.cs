using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                foreach (var journal in repository.Journals.Skip(100*i))
                {
                    yield return journal;
                }
            }
        }
    }
}
