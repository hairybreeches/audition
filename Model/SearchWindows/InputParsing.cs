using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.SearchWindows
{
    public static class InputParsing
    {
        public static IList<string> ParseStringList(string users)
        {
            return users.Split('\n')
                .Select(x => x.Trim())
                .Where(x => !String.IsNullOrEmpty(x))
                .ToList();
        }
    }
}