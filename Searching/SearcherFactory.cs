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
            new Dictionary<SearchAction, string>(),
            Enums.GetAllValues<DisplayField>());

        private readonly IDictionary<SearchAction, string> unvailableActionMessages;

        public SearcherFactory(IDictionary<SearchAction, string> unvailableActionMessages, params DisplayField[] availableFields)
        {
            this.unvailableActionMessages = unvailableActionMessages;           
            this.availableFields = availableFields;
        }

        public Searcher CreateSearcher()
        {
            return new Searcher(GetSearcher<UnusualAccountsParameters, UnusualAccountsSearcher>(SearchAction.Accounts),
                GetSearcher<EndingParameters, RoundNumberSearcher>(SearchAction.Ending),
                GetSearcher<UserParameters, UserSearcher>(SearchAction.Users));
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

        private ISearcher<TParameters> GetSearcher<TParameters, TSearcher>(SearchAction action) 
            where TParameters : ISearchParameters
            where TSearcher : ISearcher<TParameters>, new()
        {
            return SearchingSupported(action)? (ISearcher<TParameters>) new TSearcher() : new NotSupportedSearcher<TParameters>(unvailableActionMessages[action]);
            }

        private bool SearchingSupported(SearchAction searchAction)
            {
            return !unvailableActionMessages.ContainsKey(searchAction);
        }
    }
}