using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA;

/// <summary>
/// Abstraction of the WOTS+ functions defined in section 5 of FIPS 205.
/// </summary>
public interface IWots
{
    /// <summary>
    /// Generate the WOTS+ public key for ADRS <see cref="adrs"/>.
    ///
    /// FIPS 205 Section 5.1, Algorithm 5.
    /// </summary>
    /// <param name="skSeed">SK.seed from the SLH-DSA private key.</param>
    /// <param name="pkSeed">PK.seed from the SLH-DSA private key.</param>
    /// <param name="adrs">The ADRS/address of the WOTS+ public key to be generated.</param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use.</param>
    /// <returns>The generated public key</returns>
    byte[] WotsPKGen(byte[] skSeed, byte[] pkSeed, WotsHashAdrs adrs, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes);
    
    /// <summary>
    /// Sign a message <see cref="M"/> using the WOTS+ key identified by <see cref="adrs"/>.
    ///
    /// FIPS 205 Section 5.2, Algorithm 6.
    /// </summary>
    /// <param name="M">The message for which to produce a signature.</param>
    /// <param name="skSeed">SK.seed from the SLH-DSA private key</param>
    /// <param name="pkSeed">PK.seed from the SLH-DSA private key</param>
    /// <param name="adrs">The address of the WOTS+ key that is being used to sign the message.</param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use.</param>
    /// <returns>The computed message <see cref="M"/> signature.</returns>
    byte[] WotsSign(byte[] M, byte[] skSeed, byte[] pkSeed, WotsHashAdrs adrs, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes);
    
    /// <summary>
    /// Compute a candidate WOTS+ public key for ADRS <see cref="adrs"/> from a WOTS+ signature and corresponding
    /// message. Verifying a WOTS+ signature involves computing a public-key value from
    /// a message and signature value (plus more, additional steps, I believe).
    ///
    /// FIPS 205 Section 5.3, Algorithm 7.
    /// </summary>
    /// <param name="sig">The signature of <see cref="M"/></param>
    /// <param name="M">The message that was signed/whose signature is <see cref="sig"/></param>
    /// <param name="pkSeed">PK.seed from the SLH-DSA public key</param>
    /// <param name="adrs">The ADRS/address of the WOTS+ key that was used to sign <see cref="M"/>.</param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use.</param>
    /// <returns>A candidate WOTS+ public key for <see cref="adrs"/></returns>
    byte[] WotsPKFromSig(byte[] sig, byte[] M, byte[] pkSeed, WotsHashAdrs adrs, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes);
    
}
