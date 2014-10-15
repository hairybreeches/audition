using System.Collections.Generic;

namespace Model.SearchWindows
{
    public class UserParameters
    {
        public IEnumerable<string> Usernames { get; private set; }

        public UserParameters(string users)
        {
            Usernames = InputParsing.ParseStringList(users);
        }
    }
}