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
            builder.Register(_ => new[]
            {
                new ColumnFactory("Transaction ID", DisplayField.Id, line => line.TransactionId),
                new ColumnFactory("Transaction date", DisplayField.TransactionDate, line => line.TransactionDate.ToString("yyyy-MM-dd"), new DateColumnFormatter()),
                new ColumnFactory("Transaction type", DisplayField.Type, line => line.TransactionType),
                new ColumnFactory("Username", DisplayField.Username, line => line.Username),
                new ColumnFactory("Description", DisplayField.Description, line => line.Description),
                new ColumnFactory("Dr/Cr", DisplayField.LedgerEntryType, line => line.LedgerEntryType),
                new ColumnFactory("Nominal Account", DisplayField.AccountCode, line => line.NominalCode),
                new ColumnFactory("Account name", DisplayField.AccountName, line => line.NominalCodeName),
                new ColumnFactory("Amount", DisplayField.Amount, line => line.Amount)
            }).As<IEnumerable<IColumnFactory>>().As<IEnumerable<IFormatterFactory>>();
        }
    }
}
