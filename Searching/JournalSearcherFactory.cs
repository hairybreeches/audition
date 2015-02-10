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
                GetSearcher<WorkingHoursParameters, WorkingHoursSearcher>(SearchAction.Hours),
                GetSearcher<YearEndParameters, YearEndSearcher>( SearchAction.Date),
                GetSearcher<UnusualAccountsParameters, UnusualAccountsSearcher>(SearchAction.Accounts),
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

        private IJournalSearcher<TParameters> GetSearcher<TParameters, TSearcher>(SearchAction action) 
            where TParameters : ISearchParameters
            where TSearcher : IJournalSearcher<TParameters>, new()
        {
            return SearchingSupported(action)? (IJournalSearcher<TParameters>) new TSearcher() : new NotSupportedSearcher<TParameters>(unvailableActionMessages[action]);
            }

        private bool SearchingSupported(SearchAction searchAction)
            {
            return !unvailableActionMessages.ContainsKey(searchAction);
        }
    }
}