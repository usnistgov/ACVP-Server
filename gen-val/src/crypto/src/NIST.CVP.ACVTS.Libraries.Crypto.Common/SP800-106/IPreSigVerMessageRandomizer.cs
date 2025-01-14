using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.SP800_106
{
    /// <summary>
    /// Applies randomization as defined in https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-106.pdf prior
    /// to signing and/or verifying a message.
    /// </summary>
    public interface IPreSigVerMessageRandomizer
    {
        /// <summary>
        /// Randomize a message based on the original message and desired security strength.
        /// </summary>
        /// <param name="message">The message to randomize.</param>
        /// <param name="randomizationSecurityStrength">The security strength to utilize within the randomization function.</param>
        /// <returns>The randomized message.</returns>
        BitString RandomizeMessage(BitString message, int randomizationSecurityStrength);
    }
}
