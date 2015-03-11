using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using Autofac;
using Microsoft.Win32;
using Model;
using Native.Dialogs;
using Native.Disk;
using Native.RegistryAccess;
using Native.Time;
using Registry = Native.RegistryAccess.Registry;

namespace Native
{
    public class NativeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => new StaTaskScheduler(1)).SingleInstance();
            builder.Register(_ => new TaskFactory<ExportResult>(_.Resolve<StaTaskScheduler>()));
            builder.Register(_ => new TaskFactory<string>(_.Resolve<StaTaskScheduler>()));
            builder.RegisterType<FileSaveChooser>().As<IFileSaveChooser>();
            builder.RegisterType<ExcelFileChooser>().As<IExcelFileChooser>();
            builder.RegisterType<FolderChooser>().As<IFolderChooser>();
            builder.RegisterType<Clock>().As<IClock>();
            builder.RegisterType<FileSystem>().As<IFileSystem>();
            builder.RegisterType<DialogShower>();
            builder.Register(_=> new Registry(RegistryHive.CurrentUser)).As<ICurrentUserRegistry>();
            builder.Register(_=> new Registry(RegistryHive.LocalMachine)).As<ILocalMachineRegistry>();
        }
    }
}
