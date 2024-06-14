using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA;

/// <summary>
/// Abstraction of the SLH-DSA functions defined in section 9 of FIPS 205.
/// </summary>
public interface ISlhdsa
{
    /// <summary>
    /// Generates an SLH-DSA key pair
    ///
    /// FIPS 205 Section 9.1, Algorithm 17
    /// </summary>
    /// <param name="nRandomBytesForSkSeed">Random n byte value for SK.seed</param>
    /// <param name="nRandomBytesForSkPrf">Random n byte value for SK.prf</param>
    /// <param name="nRandomBytesForPkSeed">Random n byte value for PK.seed</param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use.</param>
    /// <returns>The generated SLH-DSA key pair</returns>
    KeyPair SlhKeyGen(byte[] nRandomBytesForSkSeed, byte[] nRandomBytesForSkPrf, byte[] nRandomBytesForPkSeed, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes);

    /// <summary>
    /// Generates an SLH-DSA signature w/o randomization, i.e., opt_rand is set to PK.seed
    ///
    /// FIPS 205 Section 9.2, Algorithm 18 when RANDOMIZE = false
    /// </summary>
    /// <param name="M">The message for which to produce a signature</param>
    /// <param name="sk">SLH-DSA private key</param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use</param>
    /// <returns>SLH-DSA signature</returns>
    byte[] SlhSignDeterministic(byte[] M, PrivateKey sk, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes);
    
    /// <summary>
    /// Generates an SLH-DSA signature w/ randomization, i.e., opt_rand is set to a random value
    ///
    /// FIPS 205 Section 9.2, Algorithm 18 when RANDOMIZE = true
    /// </summary>
    /// <param name="M">The message for which to produce a signature</param>
    /// <param name="sk">SLH-DSA private key</param>
    /// <param name="nRandomBytesForOptRand">Random n byte value for opt_rand</param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use</param>
    /// <returns>SLH-DSA signature</returns>
    byte[] SlhSignNonDeterministic(byte[] M, PrivateKey sk, byte[] nRandomBytesForOptRand, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes);

    /// <summary>
    /// Verify an SLH-DSA signature.
    ///
    /// FIPS 205 Section 9.3, Algorithm 19
    /// </summary>
    /// <param name="M">The message whose signature is being verified</param>
    /// <param name="sig">The purported signature of <see cref="M"/></param>
    /// <param name="pk">SLH-DSA public key</param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use</param>
    /// <returns>
    ///     <see cref="SlhdsaVerificationResult"/>
    ///     Whether the signature <see cref="sig"/> on the message <see cref="M"/> is valid.
    /// </returns>
    SlhdsaVerificationResult SlhVerify(byte[] M, byte[] sig, PublicKey pk, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes);
}
