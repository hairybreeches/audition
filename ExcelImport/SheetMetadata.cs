namespace ExcelImport
{
    public class SheetMetadata
    {
        public string Filename { get; set; }
        public int Sheet { get; set; }
        public bool UseHeaderRow { get; set; }
    }
}