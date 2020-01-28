using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures
{
    public interface IMaskFunction
    {
        BitString Mask(BitString seed, int maskLen);
    }
}