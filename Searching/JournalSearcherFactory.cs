using System.Collections.Generic;
using Model.SearchWindows;
using Persistence;

namespace Searching
{
    public class JournalSearcherFactory : IJournalSearcherFactory
    {
        private readonly ISet<SearchField> availableFields;

        public static IJournalSearcherFactory EverythingAvailable = new JournalSearcherFactory(
            SearchField.AccountCode,
            SearchField.AccountName,
            SearchField.Amount,
            SearchField.Created,
            SearchField.Description,
            SearchField.JournalDate,
            SearchField.JournalType,
            SearchField.Username);

        public JournalSearcherFactory(params SearchField[] availableFields)
        {
            this.availableFields = new HashSet<SearchField>(availableFields);
        }

        public JournalSearcher CreateJournalSearcher(IJournalRepository repository)
        {
            return new JournalSearcher(
                HoursSearchingSupported()? (IJournalSearcher<WorkingHoursParameters>) 
                    new WorkingHoursSearcher(repository) : new NotSupportedSearcher<WorkingHoursParameters>(""),

                DateSearchingSupported() ? (IJournalSearcher<YearEndParameters>) 
                    new YearEndSearcher(repository) : new NotSupportedSearcher<YearEndParameters>(""),

                AccountsSearchingSupported() ? (IJournalSearcher<UnusualAccountsParameters>) 
                    new UnusualAccountsSearcher(repository) : new NotSupportedSearcher<UnusualAccountsParameters>(""),

                EndingSearchingSupported() ? (IJournalSearcher<EndingParameters>) 
                    new RoundNumberSearcher(repository) : new NotSupportedSearcher<EndingParameters>(""),

                UserSearchingSupported() ? (IJournalSearcher<UserParameters>) 
                    new UserSearcher(repository) : new NotSupportedSearcher<UserParameters>("")
                );
        }

        public SearchCapability GetSearchCapability()
        {
            return new SearchCapability(availableFields, GetAvailableActions());
        }

        private IEnumerable<SearchAction> GetAvailableActions()
        {
            if (AccountsSearchingSupported())
            {
                yield return SearchAction.Accounts;
            }
            if (DateSearchingSupported())
            {
                yield return SearchAction.Date;
            }
            if (EndingSearchingSupported())
            {
                yield return SearchAction.Ending;
            }

            if (HoursSearchingSupported())
            {
                yield return SearchAction.Hours;
            }

            if (UserSearchingSupported())
            {
                yield return SearchAction.Users;
            }
                
        }

        private bool UserSearchingSupported()
        {
            return FieldSupported(SearchField.Username);
        }

        private bool HoursSearchingSupported()
        {
            return FieldSupported(SearchField.Created);
        }

        private bool EndingSearchingSupported()
        {
            return FieldSupported(SearchField.Amount);
        }

        private bool DateSearchingSupported()
        {
            return FieldSupported(SearchField.Created);
        }

        private bool AccountsSearchingSupported()
        {
            return FieldSupported(SearchField.AccountCode);
        }

        private bool FieldSupported(SearchField accountCode)
        {
            return availableFields.Contains(accountCode);
        }
    }
}