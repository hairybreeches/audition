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
                new ColumnFactory("Transaction ID", DisplayFieldName.Id, line => line.TransactionId),
                new ColumnFactory("Transaction date", DisplayFieldName.TransactionDate, line => line.TransactionDate.ToString("yyyy-MM-dd"), new DateColumnFormatter()),
                new ColumnFactory("Transaction type", DisplayFieldName.Type, line => line.TransactionType),
                new ColumnFactory("Username", DisplayFieldName.Username, line => line.Username),
                new ColumnFactory("Description", DisplayFieldName.Description, line => line.Description),
                new ColumnFactory("Dr/Cr", DisplayFieldName.LedgerEntryType, line => line.LedgerEntryType),
                new ColumnFactory("Nominal Account", DisplayFieldName.AccountCode, line => line.NominalCode),
                new ColumnFactory("Account name", DisplayFieldName.AccountName, line => line.NominalCodeName),
                new ColumnFactory("Amount", DisplayFieldName.Amount, line => line.Amount)
            }).As<IEnumerable<IColumnFactory>>().As<IEnumerable<IFormatterFactory>>();
        }
    }
}
