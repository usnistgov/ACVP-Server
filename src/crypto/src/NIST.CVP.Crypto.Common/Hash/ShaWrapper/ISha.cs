using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash.ShaWrapper
{
    /// <summary>
    /// Provides a SHA implementation for hashing <see cref="message"/>s
    /// </summary>
    public interface ISha
    {
        /// <summary>
        /// The <see cref="HashFunction"/> attributed to the <see cref="ISha"/> instance
        /// </summary>
        HashFunction HashFunction { get; }
        
        /// <summary>
        /// Given a <see cref="message"/>, return a <see cref="BitString"/>
        /// </summary>
        /// <param name="message">The message to hash</param>
        /// <returns></returns>
        HashResult HashMessage(BitString message);

        /// <summary>
        /// Given a <see cref="BigInteger"/>, return a digest
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        HashResult HashNumber(BigInteger number);
    }
}