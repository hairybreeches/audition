using System;

namespace Xero
{
    internal class TooManyJournalsException : Exception
    {
        public TooManyJournalsException(int maxTotalResults)
            :base(String.Format("Unable to import journals. Unfortunately, due to limits in the Xero API, we are currently unable to support ledgers with more then {0} entries", maxTotalResults))
        {            
        }
    }
}