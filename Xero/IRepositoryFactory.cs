using XeroApi;

namespace Xero
{
    internal interface IRepositoryFactory
    {
        Repository CreateRepository();
    }
}