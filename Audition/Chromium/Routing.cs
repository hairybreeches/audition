using System;

namespace Audition.Chromium
{
    //todo: share these routes between js and c# using CEF
    public static class Routing
    {        
        public const string XeroLogin = "api/xero/login";
        public const string FinishXeroLogin = "api/xero/completelogin";
        public const string Logout = "api/logout";

        public const string HoursSearch = "api/search/hours";
        public const string HoursExport = "api/export/hours";
        public const string AccountsSearch = "api/search/accounts";
        public const string AccountsExport = "api/export/accounts";
        public const string DateSearch = "api/search/date";
        public const string DateExport = "api/export/date";     
        public const string UserSearch = "api/search/users";
        public const string UserExport = "api/export/users";  
        public const string KeywordSearch = "api/search/keyword";
        public const string KeywordExport = "api/export/keyword";
        public const string EndingSearch = "api/search/ending";
        public const string EndingExport = "api/export/ending";


        public const string Openfile = "api/openfile";        
        public const string ShowDevTools = "api/devtools";
        public const string ChooseDirectory = "api/chooseDirectory";

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