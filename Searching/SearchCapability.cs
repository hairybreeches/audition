using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Searching
{
    public class SearchCapability
    {       
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public IEnumerable<SearchField> AvailableFields { get; private set; }
        public IDictionary<string, string> UnvailableActionMessages { get; private set; }        
        
        public SearchCapability(IEnumerable<SearchField> availableFields, IDictionary<SearchAction, string> unvailableActionMessages)
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

    public enum SearchAction
    {
        Users,
        Hours,
        Accounts,
        Date,
        Ending
    }

    public enum SearchField
    {
        Description,
        Username,
        Created,
        JournalDate,
        AccountCode,
        AccountName,
        Amount,
        JournalType
    }
}