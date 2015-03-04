namespace Capabilities
{
    public interface IMapping<out T>
    {
        T Description { get; }
        T Username { get; }
        T TransactionDate { get; }
        T NominalCode { get; }
        T NominalName { get; }
        T Amount { get; }
        T Id { get; }
        T Type { get; }
        T AccountCode { get; }
    }
}