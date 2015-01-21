using System.Collections.Generic;
using System.Linq;

namespace UserData
{
    public class MostRecentList
    {
        private readonly int numberToRecord;
        private IEnumerable<string> stack;

        public MostRecentList()
            :this(5)
        {
        }

        public MostRecentList(int numberToRecord)
        {
            this.numberToRecord = numberToRecord;
            stack = new List<string>();
        }

        public void AddUsage(string value)
        {
            stack = new[]{value}
                .Concat(stack)
                .Distinct()
                .Take(numberToRecord);
            
        }

        public IEnumerable<string> GetMostRecentValues()
        {
            return stack;
        }
    }
}