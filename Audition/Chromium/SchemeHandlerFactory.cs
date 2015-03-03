using CefSharp;

namespace Audition.Chromium
{
    public class SchemeHandlerFactory : ISchemeHandlerFactory
    {
        private readonly ISchemeHandler schemeHandler;

        public SchemeHandlerFactory(ISchemeHandler schemeHandler)
        {
            this.schemeHandler = schemeHandler;
        }

        public ISchemeHandler Create()
        {
            return schemeHandler;
        }
    }
}
