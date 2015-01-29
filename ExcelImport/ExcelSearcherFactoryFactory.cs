using System.Collections.Generic;
using System.Linq;
using Model;
using Searching;

namespace ExcelImport
{
    public class ExcelSearcherFactoryFactory
    {
        public IJournalSearcherFactory CreateSearcherFactory(FieldLookups lookups)
        {
            return new JournalSearcherFactory(UnvailableActionMessages(lookups), lookups.GetDisplayableFields());
        }

        private static IDictionary<SearchAction, string> UnvailableActionMessages(FieldLookups lookups)
        {
            return lookups.GetUnavailableActions()
                .Aggregate(new Dictionary<SearchAction, string>(),
                    (dictionary, action) =>
                    {
                        dictionary.Add(action, "search unavailable, please map field");
                        return dictionary;
                    });
        }
    }
}