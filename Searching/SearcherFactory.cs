using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public class SearcherFactory : ISearcherFactory
    {
        private readonly IList<DisplayField> availableFields;

        public static ISearcherFactory EverythingAvailable = new SearcherFactory(
            new Dictionary<SearchActionName, string>(),
            Enums.GetAllValues<DisplayField>());

        private readonly IDictionary<SearchActionName, string> unvailableActionMessages;

        public SearcherFactory(IDictionary<SearchActionName, string> unvailableActionMessages, params DisplayField[] availableFields)
        {
            this.unvailableActionMessages = unvailableActionMessages;           
            this.availableFields = availableFields;
        }

        public Searcher CreateSearcher()
        {
            return new Searcher(GetSearcher<UnusualAccountsParameters, UnusualAccountsSearcher>(SearchActionName.Accounts),
                GetSearcher<EndingParameters, RoundNumberSearcher>(SearchActionName.Ending),
                GetSearcher<UserParameters, UserSearcher>(SearchActionName.Users));
        }

        public SearchCapability GetSearchCapability()
        {
            return new SearchCapability(availableFields, unvailableActionMessages.Aggregate(new Dictionary<string, string>(),
                (dictionary, kvp) =>
                {
                    dictionary.Add(kvp.Key.ToString(), kvp.Value);
                    return dictionary;
                }));
        }

        private ISearcher<TParameters> GetSearcher<TParameters, TSearcher>(SearchActionName action) 
            where TParameters : ISearchParameters
            where TSearcher : ISearcher<TParameters>, new()
        {
            return SearchingSupported(action)? (ISearcher<TParameters>) new TSearcher() : new NotSupportedSearcher<TParameters>(unvailableActionMessages[action]);
            }

        private bool SearchingSupported(SearchActionName searchAction)
            {
            return !unvailableActionMessages.ContainsKey(searchAction);
        }
    }
}