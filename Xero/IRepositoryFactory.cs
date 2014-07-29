using XeroApi;

namespace Xero
{
    public interface IRepositoryFactory
    {
        IFullRepository CreateRepository();
    }
}