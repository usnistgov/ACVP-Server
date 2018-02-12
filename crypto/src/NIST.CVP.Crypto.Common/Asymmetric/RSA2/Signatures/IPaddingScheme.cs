using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2.Signatures
{
    public interface IPaddingScheme
    {
        (PublicKey key, BitString message, int nlen) PrePadCheck(PublicKey key, BitString message, int nlen);
        PaddingResult Pad(int nlen, BitString message);
        BigInteger PostSignCheck(BigInteger signature, PublicKey pubKey);
        VerifyResult VerifyPadding(int nlen, BitString message, BigInteger embededMessage, PublicKey pubKey);
    }
}
