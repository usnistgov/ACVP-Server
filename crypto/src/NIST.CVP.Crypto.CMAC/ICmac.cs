using NIST.CVP.Math;

namespace NIST.CVP.Crypto.CMAC
{
    /// <summary>
    /// Interface for generating and verifying a CMAC
    /// </summary>
    public interface ICmac
    {
        /// <summary>
        /// Generates a CMAC with the provided <see cref="key"/> and <see cref="message"/>
        /// </summary>
        /// <param name="key">The key used in the crypto operation</param>
        /// <param name="message">The message the MAC is generated from.</param>
        /// <param name="macLength">Length of the mac to generate</param>
        /// <returns>The CMAC result</returns>
        CmacResult Generate(BitString key, BitString message, int macLength);
        /// <summary>
        /// Verifies that a provided <see cref="macToVerify"/> is generated from the <see cref="key"/> and <see cref="message"/>
        /// </summary>
        /// <param name="key">The key used to generate the MAC</param>
        /// <param name="message">The message used to generate the MAC</param>
        /// <param name="macToVerify">The expected MAC that is generated from the key and message.</param>
        /// <returns>The CMAC result (actual MAC based on key/message or failure condition)</returns>
        CmacResult Verify(BitString key, BitString message, BitString macToVerify);
    }
}