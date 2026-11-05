using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash
{
    /// <summary>
    /// Provides a common one-shot interface for hash implementations.
    /// </summary>
    public interface IHash
    {
        /// <summary>
        /// Hashes the provided message.
        /// </summary>
        /// <param name="message">The message to hash.</param>
        /// <param name="outLen">
        /// The requested output length in bits for functions that support selecting it at invocation time.
        /// A value of zero uses the output length configured on the implementation.
        /// </param>
        /// <returns>The hash result.</returns>
        HashResult HashMessage(BitString message, int outLen = 0);
    }
}
