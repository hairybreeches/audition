using System;
using System.Collections.Generic;

namespace Capabilities
{
    public class SearchAction
    {
        public static readonly SearchAction Ending = new SearchAction("with round number endings", MappingFields.Amount, SearchActionName.Ending);
        public static readonly SearchAction Users = new SearchAction("posted by unexpected users", MappingFields.Username, SearchActionName.Users);
        public static readonly SearchAction Accounts = new SearchAction("posted to unusual nominal codes", MappingFields.NominalCode, SearchActionName.Accounts);

        public static IEnumerable<SearchAction> All
        {
            get
            {
                yield return Ending;
                yield return Users;
                yield return Accounts;
            }
        }

        public SearchAction(string userFriendlyDescription, IMappingField requiredField, SearchActionName name)
        {
            ErrorMessage = String.Format("In order to search for transactions {0}, you must import transactions with a value for the {1}", userFriendlyDescription, requiredField);
            RequiredField = requiredField;
            Name = name;
        }

        public IMappingField RequiredField { get; private set; }
        public string ErrorMessage { get; private set; }
        public SearchActionName Name { get; private set; }
    }
}