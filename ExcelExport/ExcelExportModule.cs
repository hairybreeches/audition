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
            builder.Register(CreateColumns).As<IEnumerable<IFormatterFactory>>();
        }

        private static ColumnFactory[] CreateColumns(IComponentContext context)
        {
            var fieldProvider = context.Resolve<DisplayFieldProvider>();

            return new[]
            {
                new ColumnFactory(fieldProvider.Id),
                new ColumnFactory(fieldProvider.TransactionDate, new DateColumnFormatter()),
                new ColumnFactory(fieldProvider.TransactionType),
                new ColumnFactory(fieldProvider.Description),
                new ColumnFactory(fieldProvider.Username),                               
                new ColumnFactory(fieldProvider.NominalCode),
                new ColumnFactory(fieldProvider.NominalName),
                new ColumnFactory(fieldProvider.LedgerEntryType),
                new ColumnFactory(fieldProvider.Amount)
            };
        }
    }
}
