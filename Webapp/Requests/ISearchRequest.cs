using Searching;

namespace Webapp.Requests
{
    public interface ISearchRequest
    {
        int PageNumber { get; }
        ISearchWindow SearchWindow { get; }
    }
}