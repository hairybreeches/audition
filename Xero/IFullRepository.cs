using System.Linq;
using XeroApi.Model;

namespace Xero
{
    public interface IFullRepository
    {
        IQueryable<Journal> Journals { get; }
    }
}