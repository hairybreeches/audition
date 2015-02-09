using Searching;
using Searching.SearchWindows;

namespace Webapp.Requests
{
    public class SearchRequest<T> where T : ISearchParameters
    {
        public SearchRequest(SearchWindow<T> searchWindow, int pageNumber)
        {
            PageNumber = pageNumber;
            SearchWindow = searchWindow;
        }


        public int PageNumber { get; private set; }
        public ISearchWindow SearchWindow { get; private set; }

        protected bool Equals(SearchRequest<T> other)
        {
            return PageNumber == other.PageNumber && Equals(SearchWindow, other.SearchWindow);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SearchRequest<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (PageNumber*397) ^ (SearchWindow != null ? SearchWindow.GetHashCode() : 0);
            }
        }
    }
}
