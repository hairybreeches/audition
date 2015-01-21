using System.Collections.Generic;
using System.Data;

namespace Sage50.Parsing
{
    public interface INominalCodeLookupFactory
    {
        NominalCodeLookup FromQueryResult(IDataReader reader);
        void AddNominalCode(Dictionary<string, string> dictionary, string nominalCode, string nominalCodeName);
    }
}