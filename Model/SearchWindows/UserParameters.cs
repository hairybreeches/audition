using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.SearchWindows
{
    public class UserParameters
    {
        public IEnumerable<string> Usernames { get; private set; }

        public UserParameters(string users)
        {
            Usernames = users.Split('\n')
                .Select(x => x.Trim())
                .Where(x => !String.IsNullOrEmpty(x))
                .ToList();
        }
    }
}