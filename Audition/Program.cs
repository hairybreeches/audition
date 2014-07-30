using System;
using Autofac;
using Xero;
using System.Windows.Forms;

namespace Audition
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var builder = new ContainerBuilder();
            builder.RegisterModule<XeroModule>();
            builder.RegisterModule<ChromiumModule>();

            using (var container = builder.Build())
            {
                var window = container.Resolve<AppForm>();                
                Application.Run(window);
            }
        }

        
    }
}
