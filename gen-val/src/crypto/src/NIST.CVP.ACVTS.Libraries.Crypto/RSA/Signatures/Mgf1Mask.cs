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
        private readonly int _outputLen = 0;
        
        public Mgf1Mask(ISha sha, int outputLen)
        {
            _sha = sha;
            _outputLen = outputLen;
        }

        public BitString Mask(BitString seed, int maskLen)
        {
            var T = new BitString(0);
            int iterations;

            // if _outputLen == 0, then Hash isn't a SHAKE
            if (_outputLen == 0)
            {
                iterations = maskLen.CeilingDivide(_sha.HashFunction.OutputLen) - 1;
            }
            else // _outputLen != 0 implies that Hash is a SHAKE. Use _outputLen in the calculation
            // instead of _sha.HashFunction.OutputLen. Since we're using an XOF, we need to specify the outputLen.
            // Otherwise the default outputLens from ShaAttributes would be used and FIPS 186-5 requires different
            // values to be used, i.e., 256 and 512
            {
                iterations = maskLen.CeilingDivide(_outputLen) - 1;
            }

            for (var i = 0; i <= iterations; i++)
            {
                var dig = _sha.HashMessage(BitString.ConcatenateBits(seed, BitString.To32BitString(i)), _outputLen).Digest;
                T = BitString.ConcatenateBits(T, dig);
            }

            return T.MSBSubstring(0, maskLen);
        }
    }
}
