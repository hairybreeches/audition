using XeroApi;

namespace Xero
{
    internal interface IRepositoryFactory
    {
        IFullRepository CreateRepository();
    }
}