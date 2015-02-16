using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;

namespace Persistence
{
    public interface ITransactionRepository
    {
        IQueryable<Transaction> GetTransactions();
        ITransactionRepository UpdateTransactions(IEnumerable<Transaction> journals);
        void ClearTransactions();
    }
}