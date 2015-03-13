using Autofac;
using CsvExport;

namespace ExcelExport
{
    public class ExcelExportModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ExcelExporter>().As<ITransactionExporter>(); 
            builder.RegisterType<ExcelFileOpener>();
        }
    }
}
