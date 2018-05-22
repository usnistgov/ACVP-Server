using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.MAC
{
    public interface IMac
    {
        /// <summary>
        /// Generates a MAC based on <see cref="key"/> and <see cref="message"/>
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="message">The message</param>
        /// <param name="macLength">Number of bits to return from the MSb of the MAC</param>
        /// <returns></returns>
        MacResult Generate(BitString key, BitString message, int macLength = 0);
        
        int OutputLength { get; }
    }
}
