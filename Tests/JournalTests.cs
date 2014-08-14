using System;
using Model;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class JournalTests
    {
        [Test]
        public void CreatingJournalWithUnbalancedEntriesGivesErrorMessageWhichListsUnbalancedLines()
        {
            var exception = Assert.Throws<InvalidJournalException>(() => new Journal(Guid.Empty, new DateTime(), new DateTime(),
                new[]
                {
                    new JournalLine("a", "a", JournalType.Cr, 42.3m),
                    new JournalLine("a", "a", JournalType.Cr, 12.3m),
                    new JournalLine("a", "a", JournalType.Dr, 54),
                }));

            var error = exception.Message;

            StringAssert.Contains("42.3", error);
            StringAssert.Contains("12.3", error);
            StringAssert.Contains("54", error);
        }
    }
}
