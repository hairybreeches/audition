using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.Accounting;

namespace Xero
{
    internal class XeroSlurper
    {
        public async Task<IEnumerable<Journal>> Slurp(IXeroJournalSource repository)
        {
            var lastTaken = 0;
            var journals = new List<XeroApi.Model.Journal>();
            for (var i = 0; i < 30; i++)
            {
                
                journals.AddRange(await GetBatch(repository, lastTaken));
                
                if (journals.Count < 100)
                    break;

                lastTaken = (int) journals.Last().JournalNumber;
            }

            return journals.Select(x=>x.ToModelJournal());
        }

        private static Task<List<XeroApi.Model.Journal>> GetBatch(IXeroJournalSource repository, int lastTaken)
        {
            return Task.Run(() => repository.Journals.Skip(lastTaken).ToList());
        }
    }
}
