using System.Collections.Generic;
using System.Linq;
using XeroApi.Model;

namespace Xero
{
    public interface IXeroJournalSource
    {
        IQueryable<Journal> Journals { get; }
    }
}