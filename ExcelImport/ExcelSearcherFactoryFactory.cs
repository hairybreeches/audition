using System.Collections.Generic;
using System.Linq;
using Model;
using Searching;

namespace ExcelImport
{
    public class ExcelSearcherFactoryFactory
    {
        private readonly ExcelDataMapper mapper;

        public ExcelSearcherFactoryFactory(ExcelDataMapper mapper)
        {
            this.mapper = mapper;
        }

        public IJournalSearcherFactory CreateSearcherFactory(FieldLookups lookups)
        {
            return new JournalSearcherFactory(UnvailableActionMessages(), mapper.GetDisplayableFields(lookups));
        }

        private static IDictionary<SearchAction, string> UnvailableActionMessages()
        {
            return new Dictionary<SearchAction, string>();
        }
    }
}