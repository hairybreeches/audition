using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.Accounting;

namespace Xero
{
    internal class XeroSlurper
    {
        private const int MaxResultsPerRequest = 100;
        private const int MaxRequests = 30;
        private const int MaxTotalResults = MaxResultsPerRequest*MaxRequests;

        public async Task<IEnumerable<Journal>> Slurp(IXeroJournalSource repository)
        {
            var lastTaken = 0;
            var journals = new List<XeroApi.Model.Journal>();
            for (var i = 0; i < MaxRequests; i++)
            {
                
                journals.AddRange(await GetBatch(repository, lastTaken));
                
                if (journals.Count < MaxResultsPerRequest)
                    break;

                lastTaken = (int) journals.Last().JournalNumber;
            }

            if (journals.Count >= MaxTotalResults)
            {
                throw new TooManyJournalsException(MaxTotalResults);
            }

            return journals.Select(x=>x.ToModelJournal());
        }

        private static Task<List<XeroApi.Model.Journal>> GetBatch(IXeroJournalSource repository, int lastTaken)
        {
            return Task.Run(() => repository.Journals.Skip(lastTaken).ToList());
        }
    }
}
