using System.Collections;
using System.Collections.Generic;
using Model.Accounting;
using Model.SearchWindows;

namespace Model
{
    public interface IJournalSearcher
    {
        IEnumerable<Journal> FindJournalsWithin(HoursSearchWindow searchWindow);
        IEnumerable<Journal> FindJournalsWithin(AccountsSearchWindow searchWindow);
    }
}
