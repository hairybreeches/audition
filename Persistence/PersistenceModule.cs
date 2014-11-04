using Autofac;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace Persistence
{
    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JournalRepository>();
            builder.Register(_ =>
            {
                var store = new EmbeddableDocumentStore {RunInMemory = true};
                store.Initialize();
                return store;
            }).As<IDocumentStore>().SingleInstance();

            builder.Register(x => x.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .InstancePerRequest();
        }
    }
}
