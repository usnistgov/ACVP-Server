using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC
{
    public interface IEccNonceProvider
    {
        BigInteger GetNonce(BigInteger privateD, BigInteger hashedMessage, BigInteger orderN);
    }
}