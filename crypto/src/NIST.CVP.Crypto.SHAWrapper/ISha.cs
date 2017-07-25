using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHAWrapper
{
    /// <summary>
    /// Provides a SHA implementation for hashing <see cref="message"/>s
    /// </summary>
    public interface ISha
    {
        /// <summary>
        /// Given a <see cref="message"/>, return a <see cref="BitString"/>
        /// </summary>
        /// <param name="message">The message to hash</param>
        /// <returns></returns>
        HashResult HashMessage(BitString message);
    }
}