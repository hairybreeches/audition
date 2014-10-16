using System.Collections.Generic;
using System.Linq;

namespace Model.SearchWindows
{
    public class UserParameters
    {
        public IEnumerable<string> Usernames { get; private set; }

        public UserParameters(string users)
        {
            Usernames = InputParsing.ParseStringList(users);
        }

        protected bool Equals(UserParameters other)
        {
            return Usernames.SequenceEqual(other.Usernames);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserParameters) obj);
        }

        public override int GetHashCode()
        {
            return (Usernames != null ? Usernames.GetHashCode() : 0);
        }
    }
}