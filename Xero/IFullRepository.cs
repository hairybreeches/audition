using System.Linq;
using XeroApi.Model;

namespace Xero
{
    internal interface IFullRepository
    {
        IQueryable<Journal> Journals { get; }
    }
}