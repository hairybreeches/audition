using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using Autofac;

namespace Native
{
    public class NativeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => new StaTaskScheduler(1)).SingleInstance();
            builder.Register(_ => new TaskFactory<string>(_.Resolve<StaTaskScheduler>()));
            builder.RegisterType<FileSaveChooser>().As<IFileSaveChooser>();
            builder.RegisterType<FolderChooser>().As<IFolderChooser>();
            builder.RegisterType<FileSystem>().As<IFileSystem>();
            builder.RegisterType<RegistryReader>().As<IRegistryReader>();
        }
    }
}
