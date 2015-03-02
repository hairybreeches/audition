using System;
using System.Collections.Generic;
using System.Linq;

namespace Capabilities
{
    public class SearchAction
    {
        private readonly string userFriendlyDescription;

        public SearchAction(string userFriendlyDescription, SearchActionName name, params IMappingField[] requiredFields)
        {
            this.userFriendlyDescription = userFriendlyDescription;            
            RequiredFields = requiredFields;
            Name = name;
        }

        public IList<IMappingField> RequiredFields { get; private set; }

        public string GetErrorMessage()
        {
            if (RequiredFields.Count == 0)
            {
                return String.Format("It is not possible to search for transactions {0}.", userFriendlyDescription);
            }

            return
                String.Format("In order to search for transactions {0}, you must import transactions with a value for the {1}",
                    userFriendlyDescription, GetRequiredFieldsList()
                    );
        }

        private string GetRequiredFieldsList()
        {
            if (RequiredFields.Count == 0)
            {
                return "";
            }
            if (RequiredFields.Count == 1)
            {
                return RequiredFields.Single().ToString();
            }

            var last = RequiredFields.Last();
            var others = RequiredFields.Take(RequiredFields.Count - 1);
            return String.Join(", for the ", others.Select(x => x.ToString())) + " and for the " + last;
        }

        public SearchActionName Name { get; private set; }
    }
}