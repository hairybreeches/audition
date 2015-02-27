using System.Collections.Generic;
using Searching;
using SqlImport;

namespace ExcelImport
{
    public class SearchAction
    {
        public static readonly SearchAction Ending = new SearchAction("In order to search for transactions with round number endings, you must import transactions with an amount value", MappingFields.Amount, SearchActionName.Ending);

        public static readonly SearchAction Users = new SearchAction("In order to search for transactions posted by unexpected users, you must import transactions with a username value", MappingFields.Username, SearchActionName.Users);

        public static readonly SearchAction Accounts = new SearchAction("In order to search for transactions posted to unusual nominal codes, you must import transactions with a nominal code value", MappingFields.NominalCode, SearchActionName.Accounts);

        public static IEnumerable<SearchAction> All
        {
            get
            {
                yield return Ending;
                yield return Users;
                yield return Accounts;
            }
        }

        public SearchAction(string errorMessage, IMappingField requiredField, SearchActionName name)
        {
            ErrorMessage = errorMessage;
            RequiredField = requiredField;
            Name = name;
        }

        public IMappingField RequiredField { get; private set; }
        public string ErrorMessage { get; private set; }
        public SearchActionName Name { get; private set; }
    }
}