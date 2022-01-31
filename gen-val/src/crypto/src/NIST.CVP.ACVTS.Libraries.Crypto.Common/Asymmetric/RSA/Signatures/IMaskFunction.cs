using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Signatures
{
    public interface IMaskFunction
    {
        BitString Mask(BitString seed, int maskLen);
    }
}
