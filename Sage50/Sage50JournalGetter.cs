using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using Model;
using Model.Accounting;
using Persistence;
using Sage50.Parsing;
using Sage50.Parsing.Schema;

namespace Sage50
{
    public class Sage50JournalGetter
    {
        private readonly JournalReader journalReader;        
        private readonly JournalSchema schema;

        public Sage50JournalGetter(JournalReader journalReader, JournalSchema schema)
        {
            this.journalReader = journalReader;            
            this.schema = schema;
        }

        public IEnumerable<Journal> GetJournals(DbConnection loginDetails)
        {
            try
            {
                return GetJournalsInner(loginDetails);
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

        private IEnumerable<Journal> GetJournalsInner(DbConnection connection)
        {
                var nominalLookup = CreateNominalCodeLookup(connection);

                return  GetJournals(connection, nominalLookup, "AUDIT_JOURNAL")
                    .Concat(GetJournals(connection, nominalLookup, "AUDIT_HISTORY_JOURNAL"));
        }

        private IEnumerable<Journal> GetJournals(DbConnection connection, NominalCodeLookup nominalLookup, string tableName)
        {
            var command = CreateCommand(connection, GetJournalsText(tableName));
            var odbcDataReader = command.ExecuteReader();
            return journalReader.GetJournals(odbcDataReader, nominalLookup);
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
            return NominalCodeLookup.FromQueryResult(reader);
        }

        private string GetJournalsText(string tableName)
        {
            return String.Format("SELECT {0} FROM {1}", String.Join(",", schema.ColumnNames), tableName);
        }
    }
}
