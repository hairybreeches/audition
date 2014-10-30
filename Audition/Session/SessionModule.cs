using Autofac;

namespace Audition.Session
{
    public class SessionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //this needs to be a single instance since it stores the current repository
            builder.RegisterType<LoginSession>().SingleInstance();
            //this needs to be single instance since it stores the current available searching functionality
            builder.RegisterType<JournalSearcherFactoryStorage>().SingleInstance();
        }
    }
}