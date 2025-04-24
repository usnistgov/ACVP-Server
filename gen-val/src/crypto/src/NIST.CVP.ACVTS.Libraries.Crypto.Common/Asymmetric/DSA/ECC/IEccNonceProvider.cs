using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC
{
    public interface IEccNonceProvider
    {
        BigInteger GetNonce(BigInteger privateD, BitString hashedMessage, BigInteger orderN);
    }
}
