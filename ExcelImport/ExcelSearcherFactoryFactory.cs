using System.Collections.Generic;
using System.Linq;
using Model;
using Searching;

namespace ExcelImport
{
    public class ExcelSearcherFactoryFactory
    {
        public IJournalSearcherFactory CreateSearcherFactory(ExcelDataMapper excelDataMapper)
        {
            return new JournalSearcherFactory(UnvailableActionMessages(excelDataMapper), excelDataMapper.GetDisplayableFields());
        }

        private static IDictionary<SearchAction, string> UnvailableActionMessages(ExcelDataMapper lookups)
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