using System;

namespace Audition.Chromium
{
    internal class InvalidHttpMethod : Exception
    {
        public InvalidHttpMethod(string method)
            :base(string.Format("Unrecognised HTTP method: " + method))
        {
            
        }
    }
}