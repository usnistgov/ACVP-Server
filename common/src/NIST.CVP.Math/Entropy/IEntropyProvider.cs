using System.Numerics;

namespace NIST.CVP.Math.Entropy
{
    /// <summary>
    /// Provides an interface for getting entropy as <see cref="BitString"/> with a specified number of bits
    /// </summary>
    public interface IEntropyProvider
    {
        /// <summary>
        /// Get Entropy
        /// </summary>
        /// <param name="numberOfBits">The number of bits to receive</param>
        /// <returns>Entropy as a <see cref="BitString"/></returns>
        BitString GetEntropy(int numberOfBits);

        BigInteger GetEntropy(BigInteger minInclusive, BigInteger maxInclusive);

        void AddEntropy(BitString entropy);
        void AddEntropy(BigInteger entropy);
    }
}