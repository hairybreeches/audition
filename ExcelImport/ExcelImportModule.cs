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
            builder.RegisterType<ExcelToSqlDataConverter>();
            builder.RegisterType<ExcelJournalReader>();
            builder.RegisterType<ExcelDemoDataSupplier>();
            builder.RegisterType<FieldLookupInterpreter>().As<ISearcherFactoryFactory>().As<IDataReaderFactory>();
        }
    }
}
