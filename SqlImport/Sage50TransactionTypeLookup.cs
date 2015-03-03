using System.Collections.Generic;
using Model;

namespace SqlImport
{
    public class Sage50TransactionTypeLookup : IValueLookup<string, string>
    {
        private readonly IDictionary<string, string> lookup = new Dictionary<string, string>
        {
            {"SI", "Sales Invoice"},
            {"SA", "Sales receipt on Account"},
            {"SC", "Sales Credit Note"},
            {"SP", "Sales Refund"},
            {"SR", "Sales Receipt"},
            {"PI", "Purchase Invoice"},
            {"PA", "Payment on Account"},
            {"PC", "Purchase Credit Note"},
            {"PR", "Purchase Refund"},
            {"PP", "Purchase Payment"},
            {"JD", "Journal Debit"},
            {"JC", "Journal Credit"},
            {"BP", "Bank Payment"},
            {"BR", "Bank Receipt"},
            {"VP", "Visa Payment"},
            {"VR", "Visa Receipt"},
            {"CP", "Cash Payment"},
            {"CR", "Cash Receipt"},
        };

        public string GetLookupValue(string key)
        {
            string convertedValue;
            if (lookup.TryGetValue(key, out convertedValue))
            {
                return convertedValue;
            }
            return key;
        }
    }
}
