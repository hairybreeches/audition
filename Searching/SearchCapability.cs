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
        public IEnumerable<SearchField> UnavailableFields { get; private set; }
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public IEnumerable<SearchAction> UnavailableActions { get; private set; }        

        //todo: can work out one of these from the other
        public SearchCapability(IEnumerable<SearchField> unavailableFields, IEnumerable<SearchAction> unavailableActions)
        {
            UnavailableFields = unavailableFields;
            UnavailableActions = unavailableActions;            
        }        
    }

    public enum SearchAction
    {
        Users
    }

    public enum SearchField
    {
        Description,
        Username
    }
}