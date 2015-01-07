using Autofac;

namespace UserData
{
    public class UserDataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserDetails>().SingleInstance();
        }
    }
}
