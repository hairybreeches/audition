namespace Webapp.Requests
{
    public class OpenFileRequest
    {
        public OpenFileRequest(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; private set; }
    }
}