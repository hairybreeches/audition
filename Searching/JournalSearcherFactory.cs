using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public class JournalSearcherFactory : IJournalSearcherFactory
    {
        private readonly IList<DisplayField> availableFields;

        public static IJournalSearcherFactory EverythingAvailable = new JournalSearcherFactory(
            new Dictionary<SearchAction, string>(),
            Enums.GetAllValues<DisplayField>());

        private readonly IDictionary<SearchAction, string> unvailableActionMessages;

        public JournalSearcherFactory(IDictionary<SearchAction, string> unvailableActionMessages, params DisplayField[] availableFields)
        {
            this.unvailableActionMessages = unvailableActionMessages;           
            this.availableFields = availableFields;
        }

        public JournalSearcher CreateJournalSearcher()
        {
            return new JournalSearcher(
                GetSearcher(() => new WorkingHoursSearcher(), SearchAction.Hours),
                GetSearcher(() => new YearEndSearcher(), SearchAction.Date),
                GetSearcher(() => new UnusualAccountsSearcher(), SearchAction.Accounts),
                GetSearcher(() => new RoundNumberSearcher(), SearchAction.Ending),
                GetSearcher(() => new UserSearcher(), SearchAction.Users));
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

        private IJournalSearcher<T> GetSearcher<T>(Func<IJournalSearcher<T>> searchFactory, SearchAction action) 
            where T : ISearchParameters
        {
            return SearchingSupported(action)? searchFactory() : new NotSupportedSearcher<T>(unvailableActionMessages[action]);
            }

        private bool SearchingSupported(SearchAction searchAction)
            {
            return !unvailableActionMessages.ContainsKey(searchAction);
        }
    }
}