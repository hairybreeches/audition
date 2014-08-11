using System;

namespace Audition.Chromium
{
    public static class WebConstants
    {
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