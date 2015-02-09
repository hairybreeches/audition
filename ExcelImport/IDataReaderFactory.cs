using SqlImport;

namespace ExcelImport
{
    public interface IDataReaderFactory
    {
        JournalDataReader GetDataReader(FieldLookups lookups);
    }
}