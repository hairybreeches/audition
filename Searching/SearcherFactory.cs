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
        private readonly ISearcher<UnusualAccountsParameters> unusualAccountsSearcher;
        private readonly ISearcher<EndingParameters> roundNumberSearcher;
        private readonly ISearcher<UserParameters> userSearcher;
        private readonly ISearcher<DuplicatePaymentsParameters> duplicatePaymentsSearcher;

        public SearcherFactory(IEnumerable<SearchAction> unavailableActions, params DisplayField[] availableFields)
        {
            this.unavailableActions = unavailableActions.ToDictionary(x=>x.Name);           
            this.availableFields = availableFields;
            unusualAccountsSearcher = new UnusualAccountsSearcher();
            roundNumberSearcher = new RoundNumberSearcher();
            userSearcher = new UserSearcher();
            duplicatePaymentsSearcher = new DuplicatePaymentsSearcher();
        }

        public Searcher CreateSearcher()
        {
            return new Searcher(GetSearcher(SearchActionName.Accounts, unusualAccountsSearcher),
                GetSearcher(SearchActionName.Ending, roundNumberSearcher),
                GetSearcher(SearchActionName.Users, userSearcher), 
                GetSearcher(SearchActionName.Duplicates, duplicatePaymentsSearcher));
        }

        public SearchCapability GetSearchCapability()
        {
            return new SearchCapability(availableFields.Select(x => x.Name).ToArray(),
                unavailableActions.Aggregate(new Dictionary<string, string>(), (dictionary, action) =>
                {
                    dictionary.Add(action.Key.ToString(), action.Value.GetErrorMessage());
                    return dictionary;
                }));
        }

        private ISearcher<TParameters> GetSearcher<TParameters>(SearchActionName action, ISearcher<TParameters> functionalSearcher) 
            where TParameters : ISearchParameters
        {
            return SearchingSupported(action)? functionalSearcher : new NotSupportedSearcher<TParameters>(unavailableActions[action].GetErrorMessage());
            }

        private bool SearchingSupported(SearchActionName searchAction)
            {
            return !unavailableActions.Keys.Contains(searchAction);
        }
    }
}