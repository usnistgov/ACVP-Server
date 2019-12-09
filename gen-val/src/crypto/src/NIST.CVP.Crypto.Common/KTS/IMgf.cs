using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KTS
{
    /// <summary>
    /// Interface for Mask Generation Function (MGF) as described in
    /// https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Br2.pdf section 7.2.2.2
    /// </summary>
    public interface IMgf
    {
        /// <summary>
        /// Generate a mask for use in KTS schemes.
        /// </summary>
        /// <param name="mgfSeed">The Mask Generation Function Seed (a random value)</param>
        /// <param name="maskLenBits">The intended length of the mask.</param>
        /// <returns></returns>
        BitString Generate(BitString mgfSeed, int maskLenBits);
    }
}