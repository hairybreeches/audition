using Autofac;

namespace Webapp.Session
{
    public class SessionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoginSession>();
            //this needs to be single instance since it stores the current available searching functionality
            builder.RegisterType<JournalSearcherFactoryStorage>().SingleInstance();
        }
    }
}