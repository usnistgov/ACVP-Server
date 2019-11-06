using System.Numerics;
using NIST.CVP.Crypto.Common.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Helpers
{
    public static class RsaKeyHelper
    {
        public const string InvalidExponentMessage = "Incorrect e, must be greater than 2^16, less than 2^256, odd";

        public static bool IsValidExponent(BigInteger e)
        {
            if (e <= NumberTheory.Pow2(16) || e >= NumberTheory.Pow2(256) || e.IsEven)
            {
                return false;
            }

            return true;
        }
    }
}