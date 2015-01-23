using System.Collections.Generic;
using Searching;

namespace ExcelImport
{
    public class ExcelImportMapping
    {
        public string Filename { get; set; }
        public bool UseHeaderRow { get; set; }
        public FieldLookups Lookups { get; set; } 
    }
}