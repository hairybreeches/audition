using System.Collections.Generic;
using System.Linq;
using Model.Accounting;

namespace Model.Searching
{
    public class JournalRepository
    {
        public IEnumerable<Journal> Journals { get; private set; }

        public JournalRepository(IEnumerable<Journal> journals)
        {
            Journals = journals.ToList();
        }
    }
}
