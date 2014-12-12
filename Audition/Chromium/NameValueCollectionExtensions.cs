using System.Collections.Generic;
using System.Collections.Specialized;

namespace Audition.Chromium
{
    public static class NameValueCollectionExtensions
    {
        public static IDictionary<string, string> ToDictionary(this NameValueCollection col)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var k in col.AllKeys)
            {
                dict.Add(k, col[k]);
            }
            return dict;
        }

        public static NameValueCollection ToNameValueCollection(this IDictionary<string, string> headers)
        {
            var collection = new NameValueCollection();
            foreach (var header in headers)
            {
                collection.Add(header.Key, header.Value);
            }

            return collection;
        }
    }
}