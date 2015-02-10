using Searching;

namespace ExcelImport
{
    public interface ISearcherFactoryFactory
    {
        IJournalSearcherFactory CreateSearcherFactory(FieldLookups lookups);
    }
}