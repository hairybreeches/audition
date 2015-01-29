using System;
using System.Collections.Generic;
using Model.SearchWindows;
using Persistence;

namespace Searching
{
    public class JournalSearcherFactory : IJournalSearcherFactory
    {
        private readonly ISet<SearchField> availableFields;

        public static IJournalSearcherFactory EverythingAvailable = new JournalSearcherFactory(
            new Dictionary<SearchAction, string>(),
            SearchField.AccountCode,
            SearchField.AccountName,
            SearchField.Amount,
            SearchField.Created,
            SearchField.Description,
            SearchField.JournalDate,
            SearchField.JournalType,
            SearchField.Username);

        private readonly IDictionary<SearchAction, string> unvailableActionMessages;

        public JournalSearcherFactory(IDictionary<SearchAction, string> unvailableActionMessages, params SearchField[] availableFields)
        {
            this.unvailableActionMessages = unvailableActionMessages;           
            this.availableFields = new HashSet<SearchField>(availableFields);
        }

        public JournalSearcher CreateJournalSearcher(IJournalRepository repository)
        {
            return new JournalSearcher(
                GetSearcher(repo => new WorkingHoursSearcher(repo), SearchAction.Hours, repository),
                GetSearcher(repo => new YearEndSearcher(repo), SearchAction.Date, repository),
                GetSearcher(repo => new UnusualAccountsSearcher(repo), SearchAction.Accounts, repository),
                GetSearcher(repo => new RoundNumberSearcher(repo), SearchAction.Ending, repository),
                GetSearcher(repo => new UserSearcher(repo), SearchAction.Users, repository));
        }

        public SearchCapability GetSearchCapability()
        {
            return new SearchCapability(availableFields, unvailableActionMessages);
        }

        private IJournalSearcher<T> GetSearcher<T>(Func<IJournalRepository, IJournalSearcher<T>> searchFactory, SearchAction action, IJournalRepository repository)
        {
            return SearchingSupported(action)? searchFactory(repository) : new NotSupportedSearcher<T>(unvailableActionMessages[action]);
            }

        private bool SearchingSupported(SearchAction searchAction)
            {
            return !unvailableActionMessages.ContainsKey(searchAction);
        }
    }
}