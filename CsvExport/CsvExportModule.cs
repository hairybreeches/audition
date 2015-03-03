using System.Collections.Generic;
using Autofac;
using Model;
using SqlImport;

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
