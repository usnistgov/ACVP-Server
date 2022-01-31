using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.Signatures
{
    public class Mgf1Mask : IMaskFunction
    {
        private readonly ISha _sha;

        public Mgf1Mask(ISha sha)
        {
            _sha = sha;
        }

        public BitString Mask(BitString seed, int maskLen)
        {
            var T = new BitString(0);
            var iterations = maskLen.CeilingDivide(_sha.HashFunction.OutputLen) - 1;

            for (var i = 0; i <= iterations; i++)
            {
                var dig = _sha.HashMessage(BitString.ConcatenateBits(seed, BitString.To32BitString(i))).Digest;
                T = BitString.ConcatenateBits(T, dig);
            }

            return T.MSBSubstring(0, maskLen);
        }
    }
}
