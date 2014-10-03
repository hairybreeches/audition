using Autofac;

namespace Audition.Session
{
    public class SessionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoginSession>().SingleInstance();
        }
    }
}