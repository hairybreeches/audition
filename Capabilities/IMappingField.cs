namespace Capabilities
{
    public interface IMappingField
    {
        string UserFriendlyName { get; }
        T GetValue<T>(IMappingVisitor<T> visitor);
    }
}