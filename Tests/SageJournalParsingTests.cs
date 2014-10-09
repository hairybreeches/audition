using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Model.Accounting;
using NUnit.Framework;
using Sage50.Parsing;

namespace Tests
{
    [TestFixture]
    public class SageJournalParsingTests
    {
        [Test]
        public void CanConvertJournal()
        {
            var reader = new JournalReader(Steve());
            var journals = reader.GetJournals().ToList();
            CollectionAssert.AreEqual(new[] { new Journal("26", DateTime.Parse("27/04/2010 17:16"), DateTime.Parse("31/12/2013"), "MANAGER", "Unpresented Cheque", new []
            {
                new JournalLine("1200", "1200", JournalType.Dr, 55), 
                new JournalLine("9998", "9998", JournalType.Cr, 55), 
                new JournalLine("2200", "2200", JournalType.Dr, 0)
            })}, journals);
        }

        public IDataReader Steve()
        {
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(GetSageColumns());
            foreach (var parameterArray in GetRows())
            {
                dataTable.Rows.Add(parameterArray);
            }
                

            return dataTable.CreateDataReader();
        }

        private static IEnumerable<object[]> GetRows()
        {
            return new[]
            {
                new[]
                {
                    "26", "BR", "31/12/2013", "1200", "1200", "1200", "56956", "MANAGER", "Unpresented Cheque", "", "0", "",
                    "N", "-", "Y", "", "1", "Sales", "T9", "0", "55", "55", "26", "26", "0", "0", "13/09/2013", "", "0",
                    "0", "0", "27/04/2010 17:16", "16/06/2014 15:09", "0"
                },
                new[]
                {
                    "26", "BR", "31/12/2013", "1200", "9998", "1200", "56956", "MANAGER", "Unpresented Cheque", "", "0", "",
                    "N", "-", "Y", "", "1", "Sales", "T9", "0", "-55", "-55", "26", "26", "0", "0", "13/09/2013", "",
                    "0", "0", "0", "27/04/2010 17:16", "16/06/2014 15:09", "0"
                },
                new[]
                {
                    "26", "BR", "31/12/2013", "1200", "2200", "1200", "56956", "MANAGER", "Unpresented Cheque", "", "0", "",
                    "N", "-", "Y", "", "1", "Sales", "T9", "0", "0", "0", "26", "26", "0", "0", "13/09/2013", "", "0",
                    "0", "0", "27/04/2010 17:16", "16/06/2014 15:09", "0"
                }
            };
        }

        private DataColumn[] GetSageColumns()
        {
            return new[]
            {
                new DataColumn("TRAN_NUMBER", typeof (Int32)),
                new DataColumn("TYPE", typeof (String)),
                new DataColumn("DATE", typeof (DateTime)),
                new DataColumn("ACCOUNT_REF", typeof (String)),
                new DataColumn("NOMINAL_CODE", typeof (String)),
                new DataColumn("BANK_CODE", typeof (String)),
                new DataColumn("INV_REF", typeof (String)),
                new DataColumn("USER_NAME", typeof (String)),
                new DataColumn("DETAILS", typeof (String)),
                new DataColumn("EXTRA_REF", typeof (String)),
                new DataColumn("DISPUTED", typeof (Int16)),
                new DataColumn("STMT_TEXT", typeof (String)),
                new DataColumn("BANK_FLAG", typeof (String)),
                new DataColumn("VAT_FLAG", typeof (String)),
                new DataColumn("PAID_FLAG", typeof (String)),
                new DataColumn("PAID_STATUS", typeof (String)),
                new DataColumn("DEPT_NUMBER", typeof (Int16)),
                new DataColumn("DEPT_NAME", typeof (String)),
                new DataColumn("TAX_CODE", typeof (String)),
                new DataColumn("DELETED_FLAG", typeof (Int16)),
                new DataColumn("AMOUNT", typeof (Double)),
                new DataColumn("FOREIGN_AMOUNT", typeof (Double)),
                new DataColumn("SPLIT_NUMBER", typeof (Int32)),
                new DataColumn("HEADER_NUMBER", typeof (Int32)),
                new DataColumn("VAT_FLAG_CODE", typeof (Int16)),
                new DataColumn("DATE_FLAG", typeof (Int16)),
                new DataColumn("DATE_ENTERED", typeof (DateTime)),
                //type is actually DBNull
                new DataColumn("VAT_RECONCILED_DATE", typeof (string)),
                new DataColumn("DISPUTE_CODE", typeof (Int16)),
                new DataColumn("FUND_ID", typeof (Int32)),
                new DataColumn("VAT_LEDGER_RETURN_ID", typeof (Int32)),
                new DataColumn("RECORD_CREATE_DATE", typeof (DateTime)),
                new DataColumn("RECORD_MODIFY_DATE", typeof (DateTime)),
                new DataColumn("RECORD_DELETED", typeof (Int16))
            };
        }
    }
}
