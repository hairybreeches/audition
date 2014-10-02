using System.Collections.Generic;
using System.Linq;
using XeroApi.Model;

namespace Xero
{
    public interface IFullRepository
    {
        IEnumerable<Journal> Journals { get; }
    }
}