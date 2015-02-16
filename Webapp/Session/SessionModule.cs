using Autofac;

namespace Webapp.Session
{
    public class SessionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Session>();
            //this needs to be single instance since it stores the current available searching functionality
            builder.RegisterType<SearcherFactoryStorage>().SingleInstance();
        }
    }
}