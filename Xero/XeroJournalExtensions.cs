using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XeroApi.Model;

namespace Xero
{
    static class XeroJournalExtensions
    {
        public static Model.Journal ToModelJournal(this Journal xeroJournal)
        {
            return new Model.Journal
            {
                Id = xeroJournal.JournalID,
                Created = xeroJournal.CreatedDateUTC,
                JournalDate = xeroJournal.JournalDate
            };
        }
    }
}