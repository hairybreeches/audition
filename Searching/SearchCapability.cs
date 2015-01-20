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
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public IEnumerable<SearchAction> AvailableActions { get; private set; }        
        
        public SearchCapability(IEnumerable<SearchField> availableFields, IEnumerable<SearchAction> availableActions)
        {
            AvailableFields = availableFields;
            AvailableActions = availableActions;            
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