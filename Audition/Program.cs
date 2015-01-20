using System;
using System.Windows.Forms;
using Autofac;

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
