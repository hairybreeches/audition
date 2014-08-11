using System;
using System.Reflection;
using Audition.Chromium;
using Audition.Controllers;
using Autofac;
using Autofac.Integration.WebApi;
using Xero;
using System.Windows.Forms;

namespace Audition
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var builder = new ContainerBuilder();
            builder.RegisterModule<XeroModule>();
            builder.RegisterModule<ChromiumModule>();            
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());            

            using (var container = builder.Build())
            {                
                var window = container.Resolve<AppForm>();                
                Application.Run(window);
            }
        }

        
    }
}
