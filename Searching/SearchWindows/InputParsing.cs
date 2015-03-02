using System;
using System.Collections.Generic;
using System.Linq;

namespace Searching.SearchWindows
{
    public static class InputParsing
    {
        public static IList<string> ParseStringList(string users)
        {
            return users.Split(new []{'\n', ',', ' ', '\t'})
                .Select(x => x.Trim())
                .Where(x => !String.IsNullOrEmpty(x))
                .ToList();
        }
    }
}