using System.Windows.Forms;
using Audition.Chromium;
using Autofac;
using NLog;

namespace Audition
{
    class ChromiumModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppForm>();
            builder.RegisterType<ChromiumControl>();
            builder.RegisterType<LogFactory>().SingleInstance();
            //todo: http://stackoverflow.com/questions/6623431/passing-in-the-type-of-the-declaring-class-for-nlog-using-autofac
            builder.Register(c => c.Resolve<LogFactory>().GetCurrentClassLogger());
        }
    }
}
