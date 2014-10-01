using System.Collections.Generic;

namespace Model.SearchWindows
{
    public class KeywordParameters
    {
        public IEnumerable<string> Keywords { get; private set; }

        public KeywordParameters(string keywords)
        {
            Keywords = InputParsing.ParseStringList(keywords);
        }
    }
}