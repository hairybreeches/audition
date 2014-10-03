using System.Collections.Generic;
using XeroApi.Model;

namespace Xero
{
    public class IdEqualityComparer : IEqualityComparer<Journal>
    {
        public bool Equals(Journal x, Journal y)
        {
            return x.JournalID.Equals(y.JournalID);
        }

        public int GetHashCode(Journal obj)
        {
            return obj.JournalID.GetHashCode();
        }
    }
}