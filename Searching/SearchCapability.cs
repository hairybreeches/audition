using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Searching
{
    public class SearchCapability
    {       
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public IEnumerable<DisplayField> AvailableFields { get; private set; }
        public IDictionary<string, string> UnvailableActionMessages { get; private set; }        
        
        public SearchCapability(IEnumerable<DisplayField> availableFields, IDictionary<SearchAction, string> unvailableActionMessages)
        {
            AvailableFields = availableFields;
            UnvailableActionMessages = unvailableActionMessages.Aggregate(new Dictionary<string, string>(),
                (dictionary, kvp) =>
                {
                    dictionary.Add(kvp.Key.ToString(), kvp.Value);
                    return dictionary;
                });
        }        
    }
}