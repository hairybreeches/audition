using System;
using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;

namespace Model.Persistence
{
    public class JournalRepository
    {
        private IEnumerable<Journal> journals;

        private IEnumerable<Journal> Journals
        {
            get
            {
                if (journals == null)
                {
                    throw new NotLoggedInException();
                }
                return journals;
            }
        }

        public IEnumerable<Journal> GetJournalsApplyingTo(DateRange period)
        {
            return Journals.ToList().Where(x => period.Contains(x.JournalDate));
        }

        public void ReplaceContents(IEnumerable<Journal> newJournals)
        {
            journals = newJournals;
        }
        
        public void Clear()
        {
            journals = null;
        }
    }
}
