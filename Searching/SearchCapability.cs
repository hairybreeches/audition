using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Searching
{
    public class SearchCapability
    {
        public static SearchCapability EverythingAvailable = new SearchCapability(
            new[]{
            SearchField.AccountCode, 
            SearchField.AccountName, 
            SearchField.Amount,
            SearchField.Created, 
            SearchField.Description, 
            SearchField.JournalDate, 
            SearchField.JournalType, 
            SearchField.Username
            }, 
            new[]
            {
                SearchAction.Accounts, 
                SearchAction.Date, 
                SearchAction.Ending, 
                SearchAction.Hours, 
                SearchAction.Users, 
            });


        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public IEnumerable<SearchField> AvailableFields { get; private set; }
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public IEnumerable<SearchAction> AvailableActions { get; private set; }        

        //todo: can work out one of these from the other
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