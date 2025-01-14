using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA;

/// <summary>
/// Abstraction of the hypertree functions defined in section 7 of FIPS 205.
/// </summary>
public interface IHypertree
{
    /// <summary>
    /// Generates a hypertree signature.
    ///
    /// FIPS 205 Section 7.1, Algorithm 11
    /// </summary>
    /// <param name="M">The message for which to produce a signature</param>
    /// <param name="skSeed">SK.seed from the SLH-DSA private key</param>
    /// <param name="pkSeed">PK.seed from the SLH-DSA private key</param>
    /// <param name="idxTree">The index of the XMSS tree at the lowest layer of the hypertree that will sign <see cref="M"/></param>
    /// <param name="idxLeaf">The index of the WOTS+ key within the XMSS tree that will sign <see cref="M"/></param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use</param>
    /// <returns>The computed message <see cref="M"/> signature</returns>
    byte[] HypertreeSign(byte[] M, byte[] skSeed, byte[] pkSeed, ulong idxTree, int idxLeaf, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes);

    /// <summary>
    /// Verifies a hypertree signature.
    ///
    /// FIPS 205 Section 7.2, Algorithm 12
    /// </summary>
    /// <param name="M">The message whose signature is being verified</param>
    /// <param name="sigHt">The hypertree signature being verified</param>
    /// <param name="pkSeed">PK.seed from the SLH-DSA public key</param>
    /// <param name="idxTree">The index of the XMSS tree at the lowest layer of the hypertree that was used to sign <see cref="M"/></param>
    /// <param name="idxLeaf">The index of the WOTS+ key within the XMSS tree used to sign <see cref="M"/></param>
    /// <param name="pkRoot">The hypertree public key, PK.root, from the SLH-DSA public key</param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use</param>
    /// <returns>true, if the computed XMSS public key of the top layer tree is the same as the known hypertree public key, PK.root</returns>
    bool HypertreeVerify(byte[] M, byte[] sigHt, byte[] pkSeed, ulong idxTree, int idxLeaf, byte[] pkRoot, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes);
}
