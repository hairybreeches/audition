using System;
using SqlImport;
using SqlImport.Schema;

namespace Sage50.Parsing
{
    public class SageJournalSchema : JournalSchema
    {
        public SageJournalSchema()
            :base(new SchemaColumn<int>("TRAN_NUMBER", 0), 
                new SchemaColumn<string>("USER_NAME", 1), 
                new SchemaColumn<DateTime>("DATE", 2), 
                new SchemaColumn<DateTime>("RECORD_CREATE_DATE", 3), 
                new SchemaColumn<string>("NOMINAL_CODE", 4), 
                new SchemaColumn<double>("AMOUNT", 5), 
                new SchemaColumn<string>("DETAILS", 6), 
                new UnmappedColumn<string>())
        {
        }
    }
}