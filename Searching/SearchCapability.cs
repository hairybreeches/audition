using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Searching
{
    public class SearchCapability
    {
        public static SearchCapability EverythingAvailable = new SearchCapability(Enumerable.Empty<SearchField>(), Enumerable.Empty<SearchAction>());


        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public IEnumerable<SearchField> unavailableFields { get; private set; }
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public IEnumerable<SearchAction> unavailableActions { get; private set; }        

        //todo: can work out one of these from the other
        public SearchCapability(IEnumerable<SearchField> unavailableFields, IEnumerable<SearchAction> unavailableActions)
        {
            this.unavailableFields = unavailableFields;
            this.unavailableActions = unavailableActions;            
        }        
    }

    public enum SearchAction
    {
        users
    }

    public enum SearchField
    {
        description,
        username
    }
}