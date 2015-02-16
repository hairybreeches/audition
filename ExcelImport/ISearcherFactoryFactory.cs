using Searching;

namespace ExcelImport
{
    public interface ISearcherFactoryFactory
    {
        ISearcherFactory CreateSearcherFactory(FieldLookups lookups);
    }
}