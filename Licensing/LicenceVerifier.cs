using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Licensing
{
    public class LicenceVerifier
    {
        public void VerifyLicence(string licenceKey)
        {
            if (!IsValid(licenceKey))
            {
                throw new InvalidLicenceKeyException();
            }                     
        }

        private static string GetExpectedCheckSum(IEnumerable<char> hashInput)
        {
            using (SHA512 shaM = new SHA512Managed())
            {
                var hash = shaM.ComputeHash(GetBytes(hashInput));
                return BitConverter.ToString(hash.Take(3).ToArray()).Replace("-", "");
            }
        }

        static byte[] GetBytes(IEnumerable<char> characters)
        {
            return characters.Select(x => (byte) x).ToArray();            
        }

        public bool IsValid(string licenceKey)
        {
            if (licenceKey.Length != 16)
            {
                return false;
            }

            var firstTenDigits = licenceKey.Take(10);

            var checkSum = GetExpectedCheckSum(firstTenDigits);

            var lastSixDigits = licenceKey.Substring(10);
            if (!Equals(checkSum, lastSixDigits))
            {
                return false;
            }

            return true;
        }
    }
}