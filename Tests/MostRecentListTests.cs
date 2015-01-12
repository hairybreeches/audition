using NUnit.Framework;
using UserData;

namespace Tests
{
    [TestFixture]
    public class MostRecentListTests
    {
        [Test]
        public void CanAddAndRetrieveAnElement()
        {
            var list = new MostRecentList();
            list.AddUsage("a");
            CollectionAssert.AreEqual(new[]{"a"}, list.GetMostRecentValues());
        }
        
        [Test]
        public void ElementsReturnedInReverseOrderOfAdding()
        {
            var list = new MostRecentList();
            list.AddUsage("a");
            list.AddUsage("b");
            list.AddUsage("c");
            list.AddUsage("d");
            list.AddUsage("e");
            CollectionAssert.AreEqual(new[]{"e", "d", "c", "b", "a"}, list.GetMostRecentValues());
        }
                
        [Test]
        public void ExcessElementsAreRemoved()
        {
            var list = new MostRecentList(3);
            list.AddUsage("a");
            list.AddUsage("b");
            list.AddUsage("c");
            list.AddUsage("d");
            list.AddUsage("e");
            CollectionAssert.AreEqual(new[]{"e", "d", "c"}, list.GetMostRecentValues());
        }      
        
        [Test]
        public void DuplicateAddsOnlyReturnedOnce()
        {
            var list = new MostRecentList(3);
            list.AddUsage("c");
            list.AddUsage("d");
            list.AddUsage("e");
            list.AddUsage("d");
            CollectionAssert.AreEqual(new[]{"d", "e", "c"}, list.GetMostRecentValues());
        }
    }
}
