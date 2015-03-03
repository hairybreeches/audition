using System;
using System.Collections.Generic;
using System.Linq;
using Capabilities;
using Model;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public class SearcherFactory : ISearcherFactory
    {
        private readonly DisplayField[] availableFields;
        private readonly Dictionary<SearchActionName, SearchAction> unavailableActions;

        public SearcherFactory(IEnumerable<SearchAction> unavailableActions, params DisplayField[] availableFields)
        {
            this.unavailableActions = unavailableActions.ToDictionary(x=>x.Name);           
            this.availableFields = availableFields;
        }

        public Searcher CreateSearcher()
        {
            return new Searcher(GetSearcher<UnusualNominalCodesParameters, UnusualNominalCodesSearcher>(SearchActionName.Accounts),
                GetSearcher<EndingParameters, RoundNumberSearcher>(SearchActionName.Ending),
                GetSearcher<UserParameters, UserSearcher>(SearchActionName.Users));
        }

        public SearchCapability GetSearchCapability()
        {
            return new SearchCapability(availableFields.Select(x => x.Name).ToArray(),
                unavailableActions.Aggregate(new Dictionary<string, string>(), (dictionary, action) =>
                {
                    dictionary.Add(action.Key.ToString(), action.Value.ErrorMessage);
                    return dictionary;
                }));
        }

        private ISearcher<TParameters> GetSearcher<TParameters, TSearcher>(SearchActionName action) 
            where TParameters : ISearchParameters
            where TSearcher : ISearcher<TParameters>, new()
        {
            return SearchingSupported(action)? (ISearcher<TParameters>) new TSearcher() : new NotSupportedSearcher<TParameters>(unavailableActions[action].ErrorMessage);
            }

        private bool SearchingSupported(SearchActionName searchAction)
            {
            return !unavailableActions.Keys.Contains(searchAction);
        }
    }
}