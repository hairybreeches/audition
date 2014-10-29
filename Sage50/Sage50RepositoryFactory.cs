using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using Model.Accounting;
using Model.Searching;
using Sage50.Parsing;
using Sage50.Parsing.Schema;

namespace Sage50
{
    public class Sage50RepositoryFactory
    {
        private readonly JournalReader journalReader;
        private readonly Sage50ConnectionFactory connectionFactory;
        private readonly JournalSchema schema;

        public Sage50RepositoryFactory(JournalReader journalReader, Sage50ConnectionFactory connectionFactory, JournalSchema schema)
        {
            this.journalReader = journalReader;
            this.connectionFactory = connectionFactory;
            this.schema = schema;
        }

        internal JournalRepository CreateJournalRepository(Sage50LoginDetails loginDetails)
        {
            using (var connection = connectionFactory.OpenConnection(loginDetails))
            {
                var nominalLookup = CreateNominalCodeLookup(connection);
                var journals = GetJournals(connection, nominalLookup, "AUDIT_JOURNAL")
                    .Concat(GetJournals(connection, nominalLookup, "AUDIT_HISTORY_JOURNAL"));
                return new JournalRepository(journals);

            }
        }

        private IList<Journal> GetJournals(OdbcConnection connection, NominalCodeLookup nominalLookup, string tableName)
        {
            var command = new OdbcCommand(GetJournalsText(tableName), connection);
            var odbcDataReader = command.ExecuteReader();
            return journalReader.GetJournals(odbcDataReader, nominalLookup).ToList();
        }

        private NominalCodeLookup CreateNominalCodeLookup(OdbcConnection connection)
        {
            var command = new OdbcCommand("SELECT ACCOUNT_REF, NAME FROM NOMINAL_LEDGER", connection);
            var reader = command.ExecuteReader();
            return NominalCodeLookup.FromQueryResult(reader);
        }

        private string GetJournalsText(string tableName)
        {
            return String.Format("SELECT {0} FROM {1}", String.Join(",", schema.ColumnNames), tableName);
        }
    }
}
