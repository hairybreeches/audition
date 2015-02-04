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
            return new JournalSearcherFactory(mapper.GetUnavailableSearchMessages(lookups), mapper.GetDisplayableFields(lookups));
        }
    }
}