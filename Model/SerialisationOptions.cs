using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SerialisationOptions
    {
        public SerialisationOptions(bool showUsername, bool showDescription)
        {
            ShowDescription = showDescription;
            ShowUsername = showUsername;
        }

        public bool ShowDescription { get; private set; }
        public bool ShowUsername { get; private set; }

        protected bool Equals(SerialisationOptions other)
        {
            return ShowDescription.Equals(other.ShowDescription) && ShowUsername.Equals(other.ShowUsername);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SerialisationOptions) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ShowDescription.GetHashCode()*397) ^ ShowUsername.GetHashCode();
            }
        }

        public override string ToString()
        {
            return String.Format("Show Description: {0}, Show username: {1}", ShowDescription, ShowUsername);
        }
    }
}
