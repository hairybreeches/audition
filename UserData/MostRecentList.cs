using System.Collections.Generic;
using System.Linq;

namespace UserData
{
    public class MostRecentList<T>
    {
        private readonly int numberToRecord;
        private IEnumerable<T> stack;

        public MostRecentList()
            :this(5)
        {
        }

        public MostRecentList(int numberToRecord)
        {
            this.numberToRecord = numberToRecord;
            stack = new List<T>();
        }

        public void AddUsage(T value)
        {
            stack = new[]{value}
                .Concat(stack)
                .Distinct()
                .Take(numberToRecord);
            
        }

        public IEnumerable<T> GetMostRecentValues()
        {
            return stack;
        }
    }
}