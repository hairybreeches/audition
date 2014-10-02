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
            var lastTaken = 0;            
            for (int i = 0; i < 30; i++)
            {
                
                var journals = repository.Journals.Skip(lastTaken).ToList();

                foreach (var journal in journals)
                {
                    yield return journal;
                }

                if (!journals.Any() || journals.Count < 100)
                    break;

                lastTaken = (int) journals.Last().JournalNumber;
            }
        }
    }
}
