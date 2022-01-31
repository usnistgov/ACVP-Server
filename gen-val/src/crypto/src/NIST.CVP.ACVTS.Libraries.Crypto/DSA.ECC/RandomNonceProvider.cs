using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DSA.ECC
{
    public class RandomNonceProvider : IEccNonceProvider
    {
        private readonly IEntropyProvider _entropyProvider;

        public RandomNonceProvider(IEntropyProvider entropyProvider)
        {
            _entropyProvider = entropyProvider;
        }

        public BigInteger GetNonce(BigInteger privateD, BigInteger hashedMessage, BigInteger orderN)
        {
            return _entropyProvider.GetEntropy(1, orderN - 1);
        }
    }
}
