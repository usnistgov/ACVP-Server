using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math.Domain;
using System.Numerics;
using NIST.CVP.Math;

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

    public enum FailureReasons
    {
        NONE,
        MESSAGE,
        E,
        SIGNATURE,
        IR_MOVED,
        IR_TRAILER
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

        public static string SaltModeToString(SaltModes saltMode)
        {
            switch (saltMode)
            {
                case SaltModes.FIXED:
                    return "fixed";
                case SaltModes.RANDOM:
                    return "random";
            }

            throw new Exception("Bad SaltMode");
        }

        public static SaltModes StringToSaltMode(string val)
        {
            if (val.ToLower().Contains("fix"))
            {
                return SaltModes.FIXED;
            }
            else
            {
                return SaltModes.RANDOM;
            }
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

        public static string SigGenModeToString(SigGenModes val)
        {
            switch (val)
            {
                case SigGenModes.ANS_931:
                    return "ansx9_31";
                case SigGenModes.PKCS_v15:
                    return "pkcs1v15";
                case SigGenModes.PSS:
                    return "pss";
            }

            throw new Exception("Bad SigGenMode");
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

        public static string FailureReasonToString(FailureReasons reason)
        {
            switch (reason)
            {
                case FailureReasons.MESSAGE:
                    return "Message modified before signing";

                case FailureReasons.E:
                    return "E modified before signing";

                case FailureReasons.SIGNATURE:
                    return "Signature modified after signing";

                case FailureReasons.IR_MOVED:
                    return "IR moved from expected location";

                case FailureReasons.IR_TRAILER:
                    return "IR trailer modified from expected value";

                default:
                case FailureReasons.NONE:
                    return "No reason to fail";
            }
        }

        public static BigInteger GetEValue()
        {
            return GetEValue(BigInteger.Pow(2, 32) + 1, BigInteger.Pow(2, 64));
        }

        public static BigInteger GetEValue(BigInteger min, BigInteger max)
        {
            var rand = new Random800_90();
            BigInteger e;
            do
            {
                e = rand.GetRandomBigInteger(min, max);
                if (e.IsEven)
                {
                    e++;
                }
            } while (e >= max);      // sanity check

            return e;
        }

        public static BitString GetSeed(int modulo)
        {
            var rand = new Random800_90();
            var security_strength = 0;
            if(modulo == 1024)
            {
                security_strength = 80;
            }
            else if (modulo == 2048)
            {
                security_strength = 112;
            }
            else if (modulo == 3072)
            {
                security_strength = 128;
            }

            return rand.GetRandomBitString(2 * security_strength);
        }

        /// <summary>
        /// Gets bitlen values for the TestCaseGenerator
        /// </summary>
        /// <param name="group"></param>
        /// <param name="rand"></param>
        /// <returns></returns>
        public static int[] GetBitlens(int modulo, KeyGenModes mode)
        {
            var rand = new Random800_90();
            var bitlens = new int[4];
            var min_single = 0;
            var max_both = 0;

            // Min_single values were given as exclusive, we add 1 to make them inclusive
            if(modulo == 1024)
            {
                // Rough estimate based on existing test vectors
                min_single = 101;
                max_both = 236;
            }
            else if (modulo == 2048)
            {
                min_single = 140 + 1;

                if (mode == KeyGenModes.B32 || mode == KeyGenModes.B34)
                {
                    max_both = 494;
                }
                else
                {
                    max_both = 750;
                }
            }
            else if (modulo == 3072)
            {
                min_single = 170 + 1;

                if (mode == KeyGenModes.B32 || mode == KeyGenModes.B34)
                {
                    max_both = 1007;
                }
                else
                {
                    max_both = 1518;
                }
            }

            bitlens[0] = rand.GetRandomInt(min_single, max_both - min_single);
            bitlens[1] = rand.GetRandomInt(min_single, max_both - bitlens[0]);
            bitlens[2] = rand.GetRandomInt(min_single, max_both - min_single);
            bitlens[3] = rand.GetRandomInt(min_single, max_both - bitlens[2]);

            return bitlens;
        }

        public static bool ValidateBitlens(int modulo, KeyGenModes mode, int[] bitlens)
        {
            if (bitlens == null)
            {
                return false;
            }

            if (bitlens.Length != 4)
            {
                return false;
            }

            var min = 0;
            var max = 0;

            if (modulo == 2048)
            {
                min = 140;

                if (mode == KeyGenModes.B32 || mode == KeyGenModes.B34)
                {
                    max = 494;
                }
                else
                {
                    max = 750;
                }
            }
            else
            {
                min = 170;

                if (mode == KeyGenModes.B32 || mode == KeyGenModes.B34)
                {
                    max = 1007;
                }
                else
                {
                    max = 1518;
                }
            }

            if (bitlens[0] < min || bitlens[1] < min || bitlens[2] < min || bitlens[3] < min)
            {
                return false;
            }

            if (bitlens[0] + bitlens[1] >= max || bitlens[2] + bitlens[3] >= max)
            {
                return false;
            }

            return true;
        }
    }
}
