using System.Collections.Generic;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;

namespace Model.Searching
{
    public interface IJournalSearcher
    {
        IEnumerable<Journal> FindJournalsWithin(SearchWindow<WorkingHours> searchWindow);
        IEnumerable<Journal> FindJournalsWithin(SearchWindow<UnusualAccountsParameters> searchWindow);
        IEnumerable<Journal> FindJournalsWithin(SearchWindow<YearEndParameters> searchWindow);
        IEnumerable<Journal> FindJournalsWithin(SearchWindow<UserParameters> searchWindow);
        IEnumerable<Journal> FindJournalsWithin(SearchWindow<KeywordParameters> searchWindow);
        IEnumerable<Journal> FindJournalsWithin(SearchWindow<EndingParameters> searchWindow);
    }
}
