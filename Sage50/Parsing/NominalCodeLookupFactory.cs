using System;
using System.Collections.Generic;
using System.Data;
using SqlImport;

namespace Sage50.Parsing
{
    public class NominalCodeLookupFactory : INominalCodeLookupFactory
    {
        public NominalCodeLookup FromQueryResult(IDataReader reader)
        {
            var dictionary = new Dictionary<string, string>();
            while (reader.Read())
            {
                var nominalCode = (string)reader[0];
                var nominalCodeName = (string)reader[1];

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
                throw new SqlDataFormatUnexpectedException(String.Format("Error adding key: {0}. Existing keys: {1}",
                    nominalCode, String.Join(", ", dictionary.Keys)), e);
            }
        }
    }
}