using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Model.Accounting;
using Sage50.Parsing;
using SqlImport;

namespace Sage50
{
    public class Sage50TransactionGetter : ISage50TransactionGetter
    {
        private readonly SageTransactionReader sageTransactionReader;        
        private readonly SageTransactionSchema schema;
        private readonly INominalCodeLookupFactory nominalCodeLookupFactory;

        public Sage50TransactionGetter(SageTransactionReader sageTransactionReader, SageTransactionSchema schema, INominalCodeLookupFactory nominalCodeLookupFactory)
        {
            this.sageTransactionReader = sageTransactionReader;            
            this.schema = schema;
            this.nominalCodeLookupFactory = nominalCodeLookupFactory;
        }

        public IEnumerable<Transaction> GetTransactions(DbConnection dbConnection, bool includeArchived)
        {
                var nominalLookup = CreateNominalCodeLookup(dbConnection);

            var unarchivedTransactions = GetJournals(dbConnection, nominalLookup, "AUDIT_JOURNAL");

            if (includeArchived)
            {
                return unarchivedTransactions
                    .Concat(GetJournals(dbConnection, nominalLookup, "AUDIT_HISTORY_JOURNAL"));
            }

            return unarchivedTransactions;
        }

        private IEnumerable<Transaction> GetJournals(DbConnection connection, NominalCodeLookup nominalLookup, string tableName)
        {
            var command = CreateCommand(connection, GetJournalsText(tableName));
            var odbcDataReader = command.ExecuteReader();
            return sageTransactionReader.GetJournals(new SqlDataReader(odbcDataReader,0 ), nominalLookup);
        }

        private DbCommand CreateCommand(DbConnection connection, string commandText)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        private NominalCodeLookup CreateNominalCodeLookup(DbConnection connection)
        {
            var command = CreateCommand(connection, "SELECT ACCOUNT_REF, NAME FROM NOMINAL_LEDGER");
            var reader = command.ExecuteReader();
            return nominalCodeLookupFactory.FromQueryResult(reader);
        }

        private string GetJournalsText(string tableName)
        {
            return String.Format("SELECT {0} FROM {1}", String.Join(",", schema.ColumnNames), tableName);
        }
    }
}
