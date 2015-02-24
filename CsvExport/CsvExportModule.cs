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
            builder.Register(_ => new[]
            {
                new ColumnFactory<SqlLedgerEntry>("Transaction ID", DisplayField.Id, line => line.TransactionId),
                new ColumnFactory<SqlLedgerEntry>("Entry time", DisplayField.Created, line => line.CreationTime),
                new ColumnFactory<SqlLedgerEntry>("Transaction date", DisplayField.TransactionDate,
                    line => line.TransactionDate.ToShortDateString()),
                new ColumnFactory<SqlLedgerEntry>("Transaction type", DisplayField.Type, line => line.TransactionType),
                new ColumnFactory<SqlLedgerEntry>("Username", DisplayField.Username, line => line.Username),
                new ColumnFactory<SqlLedgerEntry>("Description", DisplayField.Description, line => line.Description),
                new ColumnFactory<SqlLedgerEntry>("Dr/Cr", DisplayField.LedgerEntryType, line => line.LedgerEntryType),
                new ColumnFactory<SqlLedgerEntry>("Nominal Account", DisplayField.AccountCode, line => line.NominalCode),
                new ColumnFactory<SqlLedgerEntry>("Account name", DisplayField.AccountName, line => line.NominalCodeName),
                new ColumnFactory<SqlLedgerEntry>("Amount", DisplayField.Amount, line => line.Amount)
            }).As<IEnumerable<ColumnFactory<SqlLedgerEntry>>>();
        }
    }
}
