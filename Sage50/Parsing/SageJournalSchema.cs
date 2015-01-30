using System;
using SqlImport.Schema;

namespace Sage50.Parsing
{
    public class SageJournalSchema : JournalSchema
    {
        public SageJournalSchema()
            :base(
                new IntToStringDecorator(new ColumnNameVerifierDecorator<int>(new SchemaColumn<int>("TRAN_NUMBER", 0))), 
                new ColumnNameVerifierDecorator<string>(new SchemaColumn<string>("USER_NAME", 1)), 
                new ColumnNameVerifierDecorator<DateTime>(new SchemaColumn<DateTime>("DATE", 2)), 
                new ColumnNameVerifierDecorator<DateTime>(new SchemaColumn<DateTime>("RECORD_CREATE_DATE", 3)), 
                new ColumnNameVerifierDecorator<string>(new SchemaColumn<string>("NOMINAL_CODE", 4)), 
                new ColumnNameVerifierDecorator<double>(new SchemaColumn<double>("AMOUNT", 5)), 
                new ColumnNameVerifierDecorator<string>(new SchemaColumn<string>("DETAILS", 6)), 
                new UnmappedColumn<string>())
        {
        }
    }
}