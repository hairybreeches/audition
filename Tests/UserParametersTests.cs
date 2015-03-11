using System.Collections.Generic;
using NUnit.Framework;
using Searching.SearchWindows;

namespace Tests
{
    [TestFixture]
    public class UserParametersTests
    {
        [TestCaseSource("UserInput")]
        public IEnumerable<string> GetUsers(string userInput)
        {
            return new UserParameters(userInput).Usernames;
        }

        public IEnumerable<TestCaseData> UserInput
        {
            get
            {
                yield return new TestCaseData("Steve")
                    .Returns(new[]{"Steve"})
                    .SetName("Single user is returned successfully");
                
                yield return new TestCaseData("Steve\r\nBaz\r\nQux")
                    .Returns(new[]{"Steve", "Baz", "Qux"})
                    .SetName("List of users is returned successfully");
                
                yield return new TestCaseData("Steve\nBaz\nQux")
                    .Returns(new[]{"Steve", "Baz", "Qux"})
                    .SetName("Only newlines required");
                
                yield return new TestCaseData("  \t  Steve\t\n Bar\n\t Foo\t  ")
                    .Returns(new[]{"Steve", "Bar", "Foo"})
                    .SetName("Excess whitespace removed");
                
                yield return new TestCaseData("Steve,Bar,Foo")
                    .Returns(new[]{"Steve", "Bar", "Foo"})
                    .SetName("Can separate with commas");
                
                yield return new TestCaseData("Steve Bar Foo")
                    .Returns(new[]{"Steve", "Bar", "Foo"})
                    .SetName("Can separate with spaces");
                
                yield return new TestCaseData("Steve\tBar Foo,Baz Qux\nBetty,Morris")
                    .Returns(new[]{"Steve", "Bar", "Foo", "Baz", "Qux", "Betty", "Morris"})
                    .SetName("Can separate with a mix of spearators");
                
                yield return new TestCaseData("Steve\t Bar, Foo,\nBaz \tQux\n,Betty ,Morris")
                    .Returns(new[]{"Steve", "Bar", "Foo", "Baz", "Qux", "Betty", "Morris"})
                    .SetName("Repeated separators only counted once");
                
                yield return new TestCaseData("Steve,\n")
                    .Returns(new[]{"Steve"})
                    .SetName("Trailing separators ignored");
                
            }
        } 
    }
}
