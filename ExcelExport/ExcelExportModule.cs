using System;
using System.Collections.Generic;
using Autofac;
using Capabilities;
using CsvExport;
using Model;

namespace ExcelExport
{
    public class ExcelExportModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ExcelExporter>().As<ITransactionExporter>(); 
            builder.RegisterType<ExcelFileOpener>();
            builder.Register(CreateColumns).As<IEnumerable<IColumnFactory>>().As<IEnumerable<IFormatterFactory>>();
        }

        private static ColumnFactory[] CreateColumns(IComponentContext context)
        {
            var fieldProvider = context.Resolve<DisplayFieldProvider>();

            return new[]
            {
                new ColumnFactory("Transaction ID", fieldProvider.Id),
                new ColumnFactory("Transaction date", fieldProvider.TransactionDate, new DateColumnFormatter()),
                new ColumnFactory("Transaction type", fieldProvider.TransactionType),
                new ColumnFactory("Description", fieldProvider.Description),
                new ColumnFactory("Username", fieldProvider.Username),                               
                new ColumnFactory("Account code", fieldProvider.AccountCode),                               
                new ColumnFactory("Nominal Code", fieldProvider.NominalCode),
                new ColumnFactory("Nominal name", fieldProvider.NominalName),
                new ColumnFactory("Dr/Cr", fieldProvider.LedgerEntryType),
                new ColumnFactory("Amount", fieldProvider.Amount)
            };
        }
    }
}
