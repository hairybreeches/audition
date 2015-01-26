using Autofac;

namespace ExcelImport
{
    public class ExcelImportModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MetadataReader>();
            builder.RegisterType<ExcelColumnNamer>();
            builder.RegisterType<ExcelDataFileStorage>();
        }
    }
}
