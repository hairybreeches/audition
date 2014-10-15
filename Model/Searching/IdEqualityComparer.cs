using System.Collections.Generic;
using Model.Accounting;

namespace Model.Searching
{
    public class IdEqualityComparer : IEqualityComparer<Journal>
    {
        public bool Equals(Journal x, Journal y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(Journal obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}