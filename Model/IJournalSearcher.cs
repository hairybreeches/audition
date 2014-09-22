using System.Collections;
using System.Collections.Generic;

namespace Model
{
    public interface IJournalSearcher
    {
        IEnumerable<Journal> FindJournalsWithin(HoursSearchWindow searchWindow);
    }
}
