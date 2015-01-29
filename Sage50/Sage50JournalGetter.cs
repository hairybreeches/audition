using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Model.Accounting;
using Sage50.Parsing;
using Sage50.Parsing.Schema;
using SqlImport;

namespace Sage50
{
    public class Sage50JournalGetter : ISage50JournalGetter
    {
        private readonly JournalReader journalReader;        
        private readonly JournalSchema schema;
        private readonly INominalCodeLookupFactory nominalCodeLookupFactory;

        public Sage50JournalGetter(JournalReader journalReader, SageJournalSchema schema, INominalCodeLookupFactory nominalCodeLookupFactory)
        {
            this.journalReader = journalReader;            
            this.schema = schema;
            this.nominalCodeLookupFactory = nominalCodeLookupFactory;
        }

        public IEnumerable<Journal> GetJournals(DbConnection loginDetails)
        {
                var nominalLookup = CreateNominalCodeLookup(loginDetails);

                return  GetJournals(loginDetails, nominalLookup, "AUDIT_JOURNAL")
                    .Concat(GetJournals(loginDetails, nominalLookup, "AUDIT_HISTORY_JOURNAL"));
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
            return nominalCodeLookupFactory.FromQueryResult(reader);
        }

        private string GetJournalsText(string tableName)
        {
            return String.Format("SELECT {0} FROM {1}", String.Join(",", schema.ColumnNames), tableName);
        }
    }
}
