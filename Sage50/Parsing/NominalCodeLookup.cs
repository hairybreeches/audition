using System;
using System.Collections.Generic;
using SqlImport;

namespace Sage50.Parsing
{
    public class NominalCodeLookup
    {
        private readonly IDictionary<string, string> lookup;

        public NominalCodeLookup(IDictionary<string, string> lookup)
        {
            this.lookup = lookup;
        }

        public string GetNominalCodeName(string nominalCode)
        {
            string nominalCodeName;
            if (lookup.TryGetValue(nominalCode, out nominalCodeName))
            {
                return lookup[nominalCode];
            }

            throw new SqlDataFormatUnexpectedException(String.Format("Could not find lookup value for nominal code {0}, available nominal codes are {1}", nominalCode, String.Join(", ", lookup.Keys)));     
        }
    }
}