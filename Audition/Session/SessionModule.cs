using Autofac;

namespace Audition.Session
{
    public class SessionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //this needs to be a single instance since it stores the repository and the current searching functionality
            builder.RegisterType<LoginSession>().SingleInstance();
        }
    }
}