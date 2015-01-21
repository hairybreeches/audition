using System;

namespace Sage50
{
    public class SageNotInstalledException : Exception
    {
        public SageNotInstalledException()
            :base("Sage 50 doesn't seem to be installed on your computer. Please make sure that Sage is installed and try again.")
        {
        }
    }
}