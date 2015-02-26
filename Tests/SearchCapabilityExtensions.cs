using System.Collections.Generic;
using System.Linq;
using Model;
using Searching;

namespace Tests
{
    static class SearchCapabilityExtensions
    {
        private const string EndingSearchUnavailableMessage = "In order to search for transactions with round number endings, you must import transactions with an amount value";
        private const string UserSearchUnavailableMessage = "In order to search for transactions posted by unexpected users, you must import transactions with a username value";
        private const string DateUnavailableMessage = "In order to search for transactions created near or after the year end, you must import transactions with an entry time value";
        private const string AccountSearchUnavailableMessage = "In order to search for transactions posted to unusual nominal codes, you must import transactions with a nominal code value";

        public static IDictionary<string, string> WithAccountsErrorMessage(this IDictionary<string, string> dictionary)
        {
            dictionary.Add(SearchAction.Accounts.ToString(), AccountSearchUnavailableMessage);
            return dictionary;
        }
        public static IDictionary<string, string> WithUsersErrorMessage(this IDictionary<string, string> dictionary)
        {
            dictionary.Add(SearchAction.Users.ToString(), UserSearchUnavailableMessage);
            return dictionary;
        }
        public static IDictionary<string, string> WithEndingErrorMessage(this IDictionary<string, string> dictionary)
        {
            dictionary.Add(SearchAction.Ending.ToString(), EndingSearchUnavailableMessage);
            return dictionary;
        }
        public static IDictionary<string, string> WithYearEndErrorMessage(this IDictionary<string, string> dictionary)
        {
            dictionary.Add(SearchAction.Date.ToString(), DateUnavailableMessage);
            return dictionary;
        }

        public static IDictionary<string, string> WithAllErrorMessages(this IDictionary<string, string> dictionary)
        {
            dictionary.WithAccountsErrorMessage()
                .WithEndingErrorMessage()
                .WithUsersErrorMessage()
                .WithYearEndErrorMessage();

            return dictionary;
        } 

        public static IDictionary<string, string> Without(this IDictionary<string, string> dictionary, SearchAction action)
        {
            dictionary.Remove(action.ToString());
            return dictionary;
        }

        public static DisplayField[] Without(this IList<DisplayField> fields, DisplayField toRemove)
        {
            return fields.Where(x => x != toRemove).ToArray();
        }
    }
}