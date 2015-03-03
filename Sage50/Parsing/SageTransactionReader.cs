using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model;
using Model.Accounting;
using SqlImport;

namespace Sage50.Parsing
{
    public class SageTransactionReader
    {
        
        private readonly SqlFinancialTransactionReader sqlFinancialTransactionReader;
        private readonly SageTransactionSchema schema;

        public SageTransactionReader(SageTransactionSchema schema, SqlFinancialTransactionReader sqlFinancialTransactionReader)
        {
            this.schema = schema;
            this.sqlFinancialTransactionReader = sqlFinancialTransactionReader;
        }

        public IEnumerable<Transaction> GetJournals(SqlDataReader sqlDataReader, NominalCodeLookup nominalLookup)
        {
            return sqlFinancialTransactionReader.GetJournals(sqlDataReader, schema.CreateJournalReader(nominalLookup));
            
        }
    }
}