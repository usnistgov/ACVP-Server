using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math.Domain;

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

        public static PrimeTestModes? StringToPrimeTestMode(string val)
        {
            if (val.Contains("2"))
            {
                return PrimeTestModes.C2;
            }
            else if (val.Contains("3"))
            {
                return PrimeTestModes.C3;
            }
            else
            {
                return null;
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

        public static string PubExpModeToString(PubExpModes val)
        {
            switch (val)
            {
                case PubExpModes.FIXED:
                    return "fixed";
                case PubExpModes.RANDOM:
                    return "random";
            }

            throw new Exception("Bad PubExpMode");
        }

        public static string KeyGenModeToString(KeyGenModes val)
        {
            switch (val)
            {
                case KeyGenModes.B32:
                    return "B.3.2";
                case KeyGenModes.B33:
                    return "B.3.3";
                case KeyGenModes.B34:
                    return "B.3.4";
                case KeyGenModes.B35:
                    return "B.3.5";
                case KeyGenModes.B36:
                    return "B.3.6";
            }

            throw new Exception("Bad KeyGenMode");
        }

        public static string PrimeTestModeToString(PrimeTestModes val)
        {
            switch (val)
            {
                case PrimeTestModes.C2:
                    return "tblC2";
                case PrimeTestModes.C3:
                    return "tblC3";
            }

            throw new Exception("Bad PrimeTestMode");
        }
    }
}
