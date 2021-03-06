using System.Collections.Generic;

namespace Capabilities
{
    public class SearchActionProvider
    {
        public IEnumerable<SearchAction> AllSearchActions
        {
            get
            {
                yield return new SearchAction("with round number endings", SearchActionName.Ending, MappingFields.Amount);
                yield return new SearchAction("posted by unexpected users", SearchActionName.Users, MappingFields.Username);
                yield return new SearchAction("posted to unusual nominal codes", SearchActionName.NominalCodes, MappingFields.NominalCode);
				yield return new SearchAction("which are possible duplicates", SearchActionName.Duplicates, MappingFields.NominalCode, MappingFields.Amount);
            }
        }
    }
}