using System.Collections.Generic;
using System.Data.Common;
using Model.Accounting;

namespace Sage50
{
    public interface ISage50TransactionGetter
    {
        IEnumerable<Transaction> GetTransactions(DbConnection dbConnection, bool includeArchived);
    }
}