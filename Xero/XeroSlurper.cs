using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeroApi;
using XeroApi.Model;

namespace Xero
{
    public class XeroSlurper
    {
        public async Task<IEnumerable<Journal>> Slurp(Repository repository)
        {
            var lastTaken = 0;
            var journals = new List<Journal>();
            for (var i = 0; i < 30; i++)
            {
                
                journals.AddRange(await GetBatch(repository, lastTaken));
                
                if (journals.Count < 100)
                    break;

                lastTaken = (int) journals.Last().JournalNumber;
            }

            return journals;
        }

        private static Task<IEnumerable<Journal>> GetBatch(Repository repository, int lastTaken)
        {
            return Task.Run(() => (IEnumerable<Journal>) repository.Journals.Skip(lastTaken).ToList());
        }
    }
}
