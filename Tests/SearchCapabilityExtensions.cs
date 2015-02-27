using System.Collections.Generic;
using System.Linq;
using Capabilities;
using Model;
using Searching;

namespace Tests
{
    static class SearchCapabilityExtensions
    {
        private const string EndingSearchUnavailableMessage = "In order to search for transactions with round number endings, you must import transactions with a value for the amount";
        private const string UserSearchUnavailableMessage = "In order to search for transactions posted by unexpected users, you must import transactions with a value for the username";
        private const string AccountSearchUnavailableMessage = "In order to search for transactions posted to unusual nominal codes, you must import transactions with a value for the nominal code";

        public static IDictionary<string, string> WithAccountsErrorMessage(this IDictionary<string, string> dictionary)
        {
            dictionary.Add(SearchActionName.Accounts.ToString(), AccountSearchUnavailableMessage);
            return dictionary;
        }
        public static IDictionary<string, string> WithUsersErrorMessage(this IDictionary<string, string> dictionary)
        {
            dictionary.Add(SearchActionName.Users.ToString(), UserSearchUnavailableMessage);
            return dictionary;
        }
        public static IDictionary<string, string> WithEndingErrorMessage(this IDictionary<string, string> dictionary)
        {
            dictionary.Add(SearchActionName.Ending.ToString(), EndingSearchUnavailableMessage);
            return dictionary;
        }

        public static IDictionary<string, string> WithAllErrorMessages(this IDictionary<string, string> dictionary)
        {
            dictionary.WithAccountsErrorMessage()
                .WithEndingErrorMessage()
                .WithUsersErrorMessage();

            return dictionary;
        } 

        public static IDictionary<string, string> Without(this IDictionary<string, string> dictionary, SearchActionName action)
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