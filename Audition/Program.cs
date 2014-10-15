using System;
using Audition.Controllers;
using Autofac;
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
            builder.RegisterModule<AuditionModule>();       

            using (var container = builder.Build())
            {                
                var window = container.Resolve<AppForm>();                
                Application.Run(window);
            }
        }

        
    }
}
