using System;

namespace Audition.Chromium
{
    //todo: share these routes between js and c# using CEF
    public static class Routing
    {
        public const string XeroLogin = "api/xero/login";
        public const string FinishXeroLogin = "api/xero/completelogin";
        public const string Logout = "api/logout";

        public const string Search = "api/search/hours";
        public const string ExportSearch = "api/export/hours";

        public const string Openfile = "api/openfile";        
        public const string ShowDevTools = "api/devtools";

        public const string InternalDomain = @"http://localhost:1337";        

        public static string AddInternalDomain(string address)
        {
            return String.Format(@"{0}/{1}", InternalDomain, address);
        }

        public static string GetViewUrl(string location)
        {
            return AddInternalDomain("views/" + location);
        }
    }    
}