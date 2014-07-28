using System.Collections.Generic;

namespace Model
{
    public interface IJournalSearcher
    {
        IEnumerable<string> FindJournalsWithin(TimeFrame timeFrame);
    }
}
