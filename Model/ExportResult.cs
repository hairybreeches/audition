using System;

namespace Model
{
    public class ExportResult
    {
        public string Filename { get; private set; }
        public bool Completed { get; private set; }

        public ExportResult(string filename, bool completed)
        {
            Filename = filename;
            Completed = completed;
        }

        public static ExportResult Success(string fileName)
        {
            return new ExportResult(fileName, true);
        }
        
        public static ExportResult Incomplete()
        {
            return new ExportResult(String.Empty, false);
        }
    }
}