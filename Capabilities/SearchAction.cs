using System;

namespace Capabilities
{
    public class SearchAction
    {        
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