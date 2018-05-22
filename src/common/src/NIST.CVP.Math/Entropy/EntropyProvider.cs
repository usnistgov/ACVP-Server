using System.Numerics;

namespace NIST.CVP.Math.Entropy
{
    public class EntropyProvider : IEntropyProvider
    {
        private readonly IRandom800_90 _random;

        public EntropyProvider(IRandom800_90 random)
        {
            _random = random;
        }

        public BitString GetEntropy(int numberOfBits)
        {
            return _random.GetRandomBitString(numberOfBits);
        }

        public BigInteger GetEntropy(BigInteger minInclusive, BigInteger maxInclusive)
        {
            return _random.GetRandomBigInteger(minInclusive, maxInclusive);
        }

        public void AddEntropy(BitString entropy) { }
        public void AddEntropy(BigInteger entropy) { }
    }
}
