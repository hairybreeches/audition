using System;
using System.Collections.Generic;
using System.Data;

namespace Sage50.Parsing
{
    public class NominalCodeLookup
    {
        private readonly IDictionary<string, string> lookup;

        public NominalCodeLookup(IDictionary<string, string> lookup)
        {
            this.lookup = lookup;
        }

        public static NominalCodeLookup FromQueryResult(IDataReader reader)
        {
            var dictionary = new Dictionary<string, string>();
            while (reader.Read())
            {
                var nominalCode = (string) reader[0];
                var nominalCodeName = (string) reader[1];

                AddNominalCode(dictionary, nominalCode, nominalCodeName);
                
            }
            return new NominalCodeLookup(dictionary);
        }

        private static void AddNominalCode(Dictionary<string, string> dictionary, string nominalCode, string nominalCodeName)
        {
            try
            {
                dictionary.Add(nominalCode, nominalCodeName);
            }
            catch (ArgumentException e)
            {
                throw new SageDataFormatUnexpectedException(String.Format("Error adding key: {0}. Existing keys: {1}",
                    nominalCode, String.Join(", ", dictionary.Keys)), e);
            }
        }

        public string GetNominalCodeName(string nominalCode)
        {
            string nominalCodeName;
            if (lookup.TryGetValue(nominalCode, out nominalCodeName))
            {
                return lookup[nominalCode];
            }

            throw new SageDataFormatUnexpectedException(String.Format("Could not find lookup value for nominal code {0}, available nominal codes are {1}", nominalCode, String.Join(", ", lookup.Keys)));     
        }        
    }
}