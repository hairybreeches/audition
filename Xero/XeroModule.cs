using Autofac;
using Model;

namespace Xero
{
    public class XeroModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RepositoryFactory>().As<IRepositoryFactory>();
            builder.RegisterType<XeroJournalSearcher>().As<IJournalSearcher>();
        }
    }
}
