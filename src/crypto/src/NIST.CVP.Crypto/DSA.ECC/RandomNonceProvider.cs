using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DSA.ECC
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