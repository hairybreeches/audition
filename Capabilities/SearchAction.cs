using System;
using System.Collections.Generic;
using System.Linq;

namespace Capabilities
{
    public class SearchAction
    {        
        public SearchAction(string userFriendlyDescription, SearchActionName name, params IMappingField[] requiredFields)
        {
            ErrorMessage = String.Format("In order to search for transactions {0}, you must import transactions with a value for the {1}", 
                userFriendlyDescription, 
                String.Join(", the ", requiredFields.Select(x=>x.ToString())));
            RequiredFields = requiredFields;
            Name = name;
        }

        public IEnumerable<IMappingField> RequiredFields { get; private set; }
        public string ErrorMessage { get; private set; }
        public SearchActionName Name { get; private set; }
    }
}