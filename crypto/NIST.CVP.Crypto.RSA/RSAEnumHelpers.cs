using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using NIST.CVP.Crypto.SHA2;

namespace NIST.CVP.Crypto.RSA
{
    public enum KeyGenModes
    {
        B32,
        B33,
        B34,
        B35,
        B36
    }

    public enum PrimeTestModes
    {
        C2,
        C3
    }

    public enum PubExpModes
    {
        FIXED,
        RANDOM
    }

    public static class RSAEnumHelpers
    {
        public static KeyGenModes StringToKeyGenMode(string val)
        {
            if (val.Contains("3.2"))
            {
                return KeyGenModes.B32;
            }
            else if (val.Contains("3.3"))
            {
                return KeyGenModes.B33;
            }
            else if (val.Contains("3.4"))
            {
                return KeyGenModes.B34;
            }
            else if (val.Contains("3.5"))
            {
                return KeyGenModes.B35;
            }
            else
            {
                return KeyGenModes.B36;
            }
        }

        public static PrimeTestModes StringToPrimeTestMode(string val)
        {
            if (val.Contains("2"))
            {
                return PrimeTestModes.C2;
            }
            else
            {
                return PrimeTestModes.C3;
            }
        }

        public static PubExpModes StringToPubExpMode(string val)
        {
            if (val.ToLower().Contains("fix"))
            {
                return PubExpModes.FIXED;
            }
            else
            {
                return PubExpModes.RANDOM;
            }
        }
    }
}
