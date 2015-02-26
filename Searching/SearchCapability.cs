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
        public IList<DisplayField> AvailableFields { get; private set; }
        public IDictionary<string, string> UnvailableActionMessages { get; private set; }        
        
        public SearchCapability(IList<DisplayField> availableFields, IDictionary<string, string> unvailableActionMessages)
        {
            AvailableFields = availableFields;
            UnvailableActionMessages = unvailableActionMessages;
        }

        protected bool Equals(SearchCapability other)
        {
            return AvailableFields.SequenceEqual(other.AvailableFields) && UnvailableActionMessages.OrderBy(x=>x.Key).SequenceEqual(other.UnvailableActionMessages.OrderBy(x=>x.Key));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SearchCapability) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((AvailableFields != null ? AvailableFields.GetHashCode() : 0)*397) ^ (UnvailableActionMessages != null ? UnvailableActionMessages.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            var message = "Available fields: " + String.Join(", ", AvailableFields);
            return UnvailableActionMessages.Aggregate(message, 
                (currentMessage, pair) => currentMessage + "\r\n" + pair.Key + " unavailable: " + pair.Value);
        }
    }    
}