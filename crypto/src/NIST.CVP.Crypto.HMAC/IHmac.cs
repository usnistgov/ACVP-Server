using NIST.CVP.Math;

namespace NIST.CVP.Crypto.HMAC
{
    /// <summary>
    /// Interface for HMAC operations
    /// </summary>
    public interface IHmac
    {
        /// <summary>
        /// Generates a MAC based on <see cref="key"/> and <see cref="message"/>
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="message">The message</param>
        /// <param name="macLength">Number of bits to return from the MSb of the MAC</param>
        /// <returns></returns>
        HmacResult Generate(BitString key, BitString message, int macLength = 0);
    }
}