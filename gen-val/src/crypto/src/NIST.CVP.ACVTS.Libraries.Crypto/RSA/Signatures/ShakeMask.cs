using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.Signatures
{
    public class ShakeMask : IMaskFunction
    {
        private readonly ISha _shake;

        public ShakeMask(ISha shake)
        {
            _shake = shake;
        }

        public BitString Mask(BitString seed, int maskLen)
        {
            return _shake.HashMessage(seed, maskLen).Digest;
        }
    }
}
