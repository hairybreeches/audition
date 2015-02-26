using System;
using System.Collections.Generic;
using Model;
using SqlImport;

namespace Sage50.Parsing
{
    public class NominalCodeLookup : IValueLookup<string, string>
    {
        private readonly IDictionary<string, string> lookup;

        public NominalCodeLookup(IDictionary<string, string> lookup)
        {
            this.lookup = lookup;
        }

        public string GetLookupValue(string nominalCode)
        {
            string nominalCodeName;
            return lookup.TryGetValue(nominalCode, out nominalCodeName) ? lookup[nominalCode] : "<none>";
        }
    }
}