using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using Model;
using Model.Accounting;
using Model.Persistence;
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

        public InMemoryJournalRepository CreateJournalRepository(Sage50LoginDetails loginDetails)
        {
            try
            {
                return CreateJournalRepositoryInner(loginDetails);
            }
            catch (OdbcException e)
            {
                var error = e.Errors[0];
                if (error.SQLState == "08001")
                {
                    throw new IncorrectLoginDetailsException("The specified folder does not appear to be a Sage 50 data directory. The data directory can be found by logging in to Sage and clicking help->about from the menu.");
                }
                if (error.SQLState == "28000")
                {
                    throw new IncorrectLoginDetailsException("Incorrect username or password");
                }

                throw;
            }
            
        }

        private InMemoryJournalRepository CreateJournalRepositoryInner(Sage50LoginDetails loginDetails)
        {
            using (var connection = connectionFactory.OpenConnection(loginDetails))
            {
                var nominalLookup = CreateNominalCodeLookup(connection);
                var journals = GetJournals(connection, nominalLookup, "AUDIT_JOURNAL")
                    .Concat(GetJournals(connection, nominalLookup, "AUDIT_HISTORY_JOURNAL"));
                return new InMemoryJournalRepository(journals);
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
