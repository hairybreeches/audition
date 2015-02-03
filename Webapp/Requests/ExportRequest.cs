using System;
using Model.SearchWindows;

namespace Webapp.Requests
{
    public class ExportRequest<T>
    {
        public ExportRequest(SearchWindow<T> searchWindow)
        {
            SearchWindow = searchWindow;
        }


        public SearchWindow<T> SearchWindow { get; private set; }

        protected bool Equals(ExportRequest<T> other)
        {
            return Equals(SearchWindow, other.SearchWindow);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ExportRequest<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (SearchWindow != null ? SearchWindow.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return String.Format("Search window: <{0}>", SearchWindow);
        }
    }
}
