using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using Model;
using Model.Accounting;
using Model.Searching;
using Model.SearchWindows;
using Model.Time;
using Sage50.Parsing;
using Sage50.Parsing.Schema;

namespace Sage50
{
    public class Sage50JournalSearcher : IJournalSearcher
    {
        private readonly SageConnectionFactory connectionFactory;
        private readonly JournalSchema schema;
        private readonly JournalReader journalReader;

        public Sage50JournalSearcher(SageConnectionFactory connectionFactory, JournalSchema schema,JournalReader journalReader)
        {
            this.connectionFactory = connectionFactory;
            this.schema = schema;
            this.journalReader = journalReader;
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<WorkingHours> searchWindow)
        {
            return ExecuteJournalSearch(GetJournalsText());
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<UnusualAccountsParameters> searchWindow)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<YearEndParameters> searchWindow)
        {
            return ExecuteJournalSearch(GetJournalsText() + " WHERE " + GetPeriodText(searchWindow.Period) + " AND " + GetCreatedDateText(searchWindow));
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<UserParameters> searchWindow)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<KeywordParameters> searchWindow)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<EndingParameters> searchWindow)
        {
            throw new NotImplementedException();
        }

        private string GetJournalsText()
        {
            return String.Format("SELECT {0} FROM AUDIT_JOURNAL", String.Join(",", schema.ColumnNames));
        }

        private string GetPeriodText(DateRange range)
        {
            return String.Format("DATE >= '{0}' AND '{1}' >= DATE", range.From.ToString("yyyy-MM-dd"), range.To.ToString("yyyy-MM-dd"));
        }  
        
        private string GetCreatedDateText(SearchWindow<YearEndParameters> range)
        {
            return String.Format("RECORD_CREATE_DATE >= '{0}'", range.CreationStartDate().ToString("yyyy-MM-dd"));
        }

        private IEnumerable<Journal> ExecuteJournalSearch(string cmdText)
        {
            using (var connection = connectionFactory.OpenConnection())
            {
                var command = new OdbcCommand(cmdText, connection);
                return journalReader.GetJournals(command.ExecuteReader()).ToList();
            }
        }
    }
}