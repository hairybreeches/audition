namespace ExcelImport
{
    public class DataSet
    {
        public string Filename { get; set; }
        public bool UseHeaderRow { get; set; }
        public int Sheet { get; set; }
    }
}