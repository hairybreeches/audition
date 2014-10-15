using System;
using Model.Accounting;
using NUnit.Framework;
using Xero;
using XeroApi.Model;

namespace Tests
{
    [TestFixture]
    public class JournalTests
    {
        [Test]
        public void CreatingJournalFromXeroJournalWithUnbalancedEntriesGivesErrorMessageWhichListsUnbalancedLines()
        {
            var exception = Assert.Throws<InvalidJournalException>(() => new XeroApi.Model.Journal
            {
                JournalID = Guid.Empty,
                CreatedDateUTC = new DateTime(),
                JournalDate = new DateTime(),
                JournalLines = new JournalLines()
                {
                    new XeroApi.Model.JournalLine
                    {
                        NetAmount = -42.3m
                    },        
                    new XeroApi.Model.JournalLine
                    {
                        NetAmount = 12.3m
                    },                  
                    new XeroApi.Model.JournalLine
                    {
                        NetAmount = 54
                    },
                }

            }.ToModelJournal());

            var error = exception.Message;

            StringAssert.Contains("42.3", error);
            StringAssert.Contains("12.3", error);
            StringAssert.Contains("54", error);
        }
    }
}
