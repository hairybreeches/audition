using System;

namespace Licensing
{
    public class InvalidLicenceKeyException : Exception
    {
        public InvalidLicenceKeyException()
            :base("The supplied licence key is invalid. Please check the licence key and try again")
        {
        }
    }
}