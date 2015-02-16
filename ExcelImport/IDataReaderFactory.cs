using SqlImport;

namespace ExcelImport
{
    public interface IDataReaderFactory
    {
        TransactionFieldReader GetDataReader(FieldLookups lookups);
    }
}