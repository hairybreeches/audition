using Autofac;

namespace UserData
{
    public class UserDataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserDetailsStorage>().As<IUserDetailsStorage>();
        }
    }
}
