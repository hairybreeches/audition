using Autofac;

namespace CsvExport
{
    public class CsvExportModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SpreadsheetExporter>().As<ITransactionExporter>();            
            builder.RegisterType<CsvWriterFactory>().As<ISpreadsheetWriterFactory>();            
        }
    }
}
