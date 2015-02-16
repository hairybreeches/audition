using System.Collections.Generic;
using System.Data.Common;
using Model.Accounting;

namespace Sage50
{
    public interface ISage50JournalGetter
    {
        IEnumerable<Transaction> GetJournals(DbConnection dbConnection);
    }
}