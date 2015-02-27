namespace SqlImport
{
    public interface IMappingVisitor<out T>
    {
        T Description { get; }
        T Username { get; }
        T TransactionDate { get; }
        T AccountCode { get; }
        T AccountName { get; }
        T Amount { get; }
        T Id { get; }
        T Type { get; }
    }
}