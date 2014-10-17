using System;
using System.Data.Odbc;
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
                var command = new OdbcCommand(GetJournalsText(), connection);
                var journals = journalReader.GetJournals(command.ExecuteReader(), nominalLookup);
                return new JournalRepository(journals);

            }
        }

        private NominalCodeLookup CreateNominalCodeLookup(OdbcConnection connection)
        {
            var command = new OdbcCommand("SELECT ACCOUNT_REF, NAME FROM NOMINAL_LEDGER", connection);
            var reader = command.ExecuteReader();
            return NominalCodeLookup.FromQueryResult(reader);
        }

        private string GetJournalsText()
        {
            return String.Format("SELECT {0} FROM AUDIT_JOURNAL", String.Join(",", schema.ColumnNames));
        }
    }
}
