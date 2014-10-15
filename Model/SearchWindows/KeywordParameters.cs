using System.Collections.Generic;
using System.Linq;

namespace Model.SearchWindows
{
    public class KeywordParameters
    {
        public IEnumerable<string> Keywords { get; private set; }

        public KeywordParameters(string keywords)
        {
            Keywords = InputParsing.ParseStringList(keywords);
        }

        protected bool Equals(KeywordParameters other)
        {
            return Keywords.SequenceEqual(other.Keywords);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((KeywordParameters) obj);
        }

        public override int GetHashCode()
        {
            return (Keywords != null ? Keywords.GetHashCode() : 0);
        }
    }
}