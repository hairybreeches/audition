using System.Collections.Generic;

namespace Capabilities
{
    public class SearchActionProvider
    {
        public IEnumerable<SearchAction> AllSearchActions
        {
            get
            {
                yield return new SearchAction("with round number endings", MappingFields.Amount, SearchActionName.Ending);
                yield return new SearchAction("posted by unexpected users", MappingFields.Username, SearchActionName.Users);
                yield return new SearchAction("posted to unusual nominal codes", MappingFields.NominalCode, SearchActionName.Accounts);
            }
        }
    }
}