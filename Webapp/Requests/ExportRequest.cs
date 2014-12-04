using System;
using Model;
using Model.SearchWindows;

namespace Webapp.Requests
{
    public class ExportRequest<T>
    {
        public ExportRequest(SearchWindow<T> searchWindow, SerialisationOptions serialisationOptions)
        {
            SerialisationOptions = serialisationOptions;
            SearchWindow = searchWindow;
        }


        public SerialisationOptions SerialisationOptions { get; private set; }
        public SearchWindow<T> SearchWindow { get; private set; }

        protected bool Equals(ExportRequest<T> other)
        {
            return Equals(SerialisationOptions, other.SerialisationOptions) && Equals(SearchWindow, other.SearchWindow);
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
                return ((SerialisationOptions != null ? SerialisationOptions.GetHashCode() : 0)*397) ^ (SearchWindow != null ? SearchWindow.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return String.Format("Search window: <{0}>, Serialisation options: <{1}>", SearchWindow, SerialisationOptions);
        }
    }
}
