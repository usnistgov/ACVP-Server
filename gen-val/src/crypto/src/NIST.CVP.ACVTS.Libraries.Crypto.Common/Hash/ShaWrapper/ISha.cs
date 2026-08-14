using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Math.LargeBitString;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper
{
    /// <summary>
    /// Provides a SHA implementation for hashing messages.
    /// </summary>
    public interface ISha : IHash
    {
        /// <summary>
        /// The <see cref="HashFunction"/> attributed to the <see cref="ISha"/> instance
        /// </summary>
        HashFunction HashFunction { get; }

        /// <summary>
        /// Given a <see cref="BigInteger"/>, return a digest
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        HashResult HashNumber(BigInteger number);

        /// <summary>
        /// Initializes the hash function
        /// </summary>
        void Init();

        /// <summary>
        /// Provides content to hash
        /// </summary>
        /// <param name="message"></param>
        /// <param name="bitLength"></param>
        void Update(byte[] message, int bitLength);

        /// <summary>
        /// Provides content to hash, MUST not have other updates of incomplete bytes
        /// </summary>
        /// <param name="message"></param>
        /// <param name="bitLength">MUST be a multiple of 8 bits</param>
        void Update(int message, int bitLength);
        
        /// <summary>
        /// Get the hash result
        /// </summary>
        /// <param name="output"></param>
        /// <param name="outputBitLength"></param>
        /// <returns></returns>
        void Final(byte[] output, int outputBitLength = 0);

        /// <summary>
        /// Hash multiple GB at a time
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        HashResult HashLargeMessage(LargeBitString message);
    }
}
