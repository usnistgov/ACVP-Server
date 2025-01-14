using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA;

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
    KeyPair SlhKeyGen(byte[] nRandomBytesForSkSeed, byte[] nRandomBytesForSkPrf, byte[] nRandomBytesForPkSeed);
    
    public byte[] Sign(byte[] sk, byte[] message, byte[] rnd);
    
    /// <summary>
    /// Verify an SLH-DSA signature.
    ///
    /// FIPS 205 Section 9.3, Algorithm 19
    /// </summary>
    /// <param name="pk">SLH-DSA public key</param>
    /// <param name="M">The message whose signature is being verified</param>
    /// <param name="sig">The purported signature of <see cref="M"/></param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use</param>
    /// <returns>
    ///     Whether the signature <see cref="sig"/> on the message <see cref="M"/> is valid.
    /// </returns>
    bool Verify(byte[] pk, byte[] M, byte[] sig);
}
