using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys
{
    public interface IKeyGenParameterHelper
    {
        KeyPair ConvertStandardToCrt(KeyPair key);
        BitString GetEValue(int minLen, int maxLen);
        BitString GetSeed(int modulo);
        int[] GetBitlens(int modulo, PrimeGenModes mode);
    }
}