using Autofac;

namespace Persistence
{
    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //this is the in memory storage, so there can only be one instance
            builder.RegisterType<TempFileJournalRepository>().SingleInstance().As<IJournalRepository>();
        }
    }
}
