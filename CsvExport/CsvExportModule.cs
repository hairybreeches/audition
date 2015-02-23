using Autofac;

namespace CsvExport
{
    public class CsvExportModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CsvExporter>().AsSelf();           
            builder.RegisterType<CsvWriterFactory>().As<ISpreadsheetWriterFactory>();            
        }
    }
}
