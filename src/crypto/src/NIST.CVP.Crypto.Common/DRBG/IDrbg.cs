using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.DRBG
{
    /// <summary>
    /// Interface for a DRBG implementation
    /// </summary>
    public interface IDrbg
    {
        /// <summary>
        /// Instantiate the DRBG
        /// </summary>
        /// <param name="requestedSecurityStrength">The requested security strength</param>
        /// <param name="personalizationString">The personalization String</param>
        /// <returns><see cref="DrbgStatus"/></returns>
        DrbgStatus Instantiate(int requestedSecurityStrength, BitString personalizationString);
        /// <summary>
        /// Reseeds the DRBG state utilizing the passed in <see cref="additionalInput"/> and new entropy
        /// </summary>
        /// <param name="additionalInput">The additional input to get worked into the DRBG state</param>
        /// <returns><see cref="DrbgStatus"/></returns>
        DrbgStatus Reseed(BitString additionalInput);
        /// <summary>
        /// Generate bits from the DRBG.
        /// </summary>
        /// <param name="requestedNumberOfBits">The requested number of bits to generate</param>
        /// <param name="additionalInput">Additional input to work into the DRBG state</param>
        /// <returns><see cref="DrbgResult"/></returns>
        DrbgResult Generate(int requestedNumberOfBits, BitString additionalInput);
        /// <summary>
        /// Unsets the DRBG state and internals
        /// </summary>
        void Uninstantiate();
    }
}