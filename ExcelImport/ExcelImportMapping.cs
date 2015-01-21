using System.Collections.Generic;
using Searching;

namespace ExcelImport
{
    public class ExcelImportMapping
    {
        public string Filename { get; set; }
        public Dictionary<SearchField, string> Lookups { get; set; } 
    }
}