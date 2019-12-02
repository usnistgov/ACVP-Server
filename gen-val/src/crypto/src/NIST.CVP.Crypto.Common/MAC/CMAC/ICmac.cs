using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.MAC.CMAC
{
    /// <summary>
    /// Interface for generating and verifying a CMAC
    /// </summary>
    public interface ICmac : IMac
    {
        /// <summary>
        /// Verifies that a provided <see cref="macToVerify"/> is generated from the <see cref="key"/> and <see cref="message"/>
        /// </summary>
        /// <param name="key">The key used to generate the MAC</param>
        /// <param name="message">The message used to generate the MAC</param>
        /// <param name="macToVerify">The expected MAC that is generated from the key and message.</param>
        /// <returns>The CMAC result (actual MAC based on key/message or failure condition)</returns>
        MacResult Verify(BitString key, BitString message, BitString macToVerify);
    }
}