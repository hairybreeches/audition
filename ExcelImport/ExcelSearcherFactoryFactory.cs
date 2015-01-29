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
            return new JournalSearcherFactory(UnvailableActionMessages(lookups), GetDisplayFields(lookups));
        }

        private static IDictionary<SearchAction, string> UnvailableActionMessages(FieldLookups lookups)
        {
            return Enums.GetAllValues<SearchAction>()
                .Where(x => !lookups.IsSearchable(x))
                .Aggregate(new Dictionary<SearchAction, string>(),
                    (dictionary, action) =>
                    {
                        dictionary.Add(action, "search unavailable, please map field");
                        return dictionary;
                    });
        }

        private static DisplayField[] GetDisplayFields(FieldLookups lookups)
        {
            return Enums.GetAllValues<DisplayField>()
                .Where(lookups.IsDisplayable).ToArray();
        }
    }
}