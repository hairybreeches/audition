using Autofac;

namespace SqlImport
{
    public class SqlImportModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LedgerEntryParser>();
            builder.RegisterType<SqlFinancialTransactionReader>();
            builder.RegisterType<TabularFormatConverter>();
        }
    }
}
