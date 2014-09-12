using System;

namespace Audition.Chromium
{
    public static class Routing
    {
        public const string XeroLogout = "api/xero/logout";
        public const string FinishXeroLogin = "api/xero/completelogin";
        public const string XeroLogin = "api/xero/login";
        public const string ExportSearch = "api/search/export";
        public const string Openfile = "api/openfile";
        public const string Search = "api/search";

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