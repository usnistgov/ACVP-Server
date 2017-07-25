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
        C3,
        NONE
    }

    public enum PubExpModes
    {
        FIXED,
        RANDOM
    }

    public enum SigGenModes
    {
        ANS_931,
        PKCS_v15,
        PSS
    }

    public enum SaltModes
    {
        FIXED,
        RANDOM
    }

    public static class RSAEnumHelpers
    {
        public static KeyGenModes StringToKeyGenMode(string val)
        {
            if (val.Contains("3.2") || val.Contains("ProvRP"))
            {
                return KeyGenModes.B32;
            }
            else if (val.Contains("3.3") || val.Contains("Probable Random Primes"))
            {
                return KeyGenModes.B33;
            }
            else if (val.Contains("3.4") || val.Contains("ProvPC"))
            {
                return KeyGenModes.B34;
            }
            else if (val.Contains("3.5") || val.Contains("BothPC"))
            {
                return KeyGenModes.B35;
            }
            else // ( || val.Contains("ProbPC"))
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
            else if (val.Contains("3"))
            {
                return PrimeTestModes.C3;
            }
            else
            {
                return PrimeTestModes.NONE;
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
                case PrimeTestModes.NONE:
                    return "";
            }

            throw new Exception("Bad PrimeTestMode");
        }

        public static SigGenModes StringToSigGenMode(string val)
        {
            if(val.ToLower().Contains("ans"))
            {
                return SigGenModes.ANS_931;
            }

            if (val.ToLower().Contains("pkcs"))
            {
                return SigGenModes.PKCS_v15;
            }

            if (val.ToLower().Contains("pss"))
            {
                return SigGenModes.PSS;
            }

            throw new Exception("Bad SigGenMode");
        }
    }
}
