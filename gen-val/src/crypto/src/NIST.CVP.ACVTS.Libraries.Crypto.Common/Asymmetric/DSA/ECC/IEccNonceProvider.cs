using System.Numerics;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC
{
    public interface IEccNonceProvider
    {
        BigInteger GetNonce(BigInteger privateD, BigInteger hashedMessage, BigInteger orderN);
    }
}
