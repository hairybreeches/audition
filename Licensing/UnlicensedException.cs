using System;

namespace Licensing
{
    public class UnlicensedException : Exception
    {
        public UnlicensedException()
            :base("Your trial has expired and the product is unlicensed. Please contact sales@auditionsoftware.com to obtain a licence key or extend your trial.")
        {
        }
    }
}