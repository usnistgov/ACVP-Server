using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA;

/// <summary>
/// Abstraction of the FORS functions defined in section 8 of FIPS 205.
/// </summary>
public interface IFors
{
    /// <summary>
    /// Generates a FORS private-key value.
    ///
    /// FIPS 205 Section 8.1, Algorithm 13
    /// </summary>
    /// <param name="skSeed">Secret seed SK.seed from the SLH-DSA private key</param>
    /// <param name="pkSeed">Public seed PK.seed from the SLH-DSA private key</param>
    /// <param name="adrs">The address of the XMSS tree and WOTS+ key (within the XMSS tree) used to sign the FORS key in
    /// use. The address's component tree and key pair addresses also uniquely identify the FORS key in use (I believe).</param>
    /// <param name="idx">The index of the FORS secret value within the sets of FORS trees. The FORS secret value this
    /// function will compute and return.</param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use.</param>
    /// <returns>FORS private key value</returns>
    byte[] ForsSkGen(byte[] skSeed, byte[] pkSeed, ForsTreeAdrs adrs, int idx, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes);
    
    /// <summary>
    ///  Computes the nodes of a FORS tree. "Each node in the Merkle (FORS) tree is the root of a subtree, and Algorithm 14 computes
    /// the root of the subtree recursively."
    ///
    /// FIPS 205 Section 8.2, Algorithm 14
    /// </summary>
    /// <param name="skSeed">Secret seed SK.seed from the SLH-DSA private key</param>
    /// <param name="pkSeed">Public seed PK.seed from the SLH-DSA private key</param>
    /// <param name="i">the index value for the node to be calculated</param>
    /// <param name="z">the height value for the node to be calculated</param>
    /// <param name="adrs">"address" containing the tree address of the XMSS tree that signs the FORS key and the key pair address of the WOTS+ key within the XMSS tree that signs the FORS key.</param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use.</param>
    /// <returns>The n-byte root node associated with <see cref="adrs"/>, <see cref="i"/>, and <see cref="z"/></returns>
    byte[] ForsNode(byte[] skSeed, byte[] pkSeed, int i, int z, ForsTreeAdrs adrs, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes);

    /// <summary>
    /// Generates a FORS signature.
    ///
    /// FIPS 205 Section 8.3, Algorithm 15 
    /// </summary>
    /// <param name="md">the message digest to sign</param>
    /// <param name="skSeed">Secret seed SK.seed from the SLH-DSA private key</param>
    /// <param name="pkSeed">Public seed PK.seed from the SLH-DSA private key</param>
    /// <param name="adrs">"address" containing the tree address of the XMSS tree that signs the FORS key and the key pair address of the WOTS+ key within the XMSS tree that signs the FORS key.</param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use.</param>
    /// <returns>The computed message digest signature</returns>
    byte[] ForsSign(byte[] md, byte[] skSeed, byte[] pkSeed, ForsTreeAdrs adrs, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes);

    /// <summary>
    /// Compute a candidate FORS public key from a FORS signature and corresponding message digest
    ///
    /// FIPS 205 Section 8.4, Algorithm 16
    /// </summary>
    /// <param name="sigFors">The purported signature of <see cref="md"/></param>
    /// <param name="md">The message digest that was signed/whose signature is <see cref="sigFors"/></param>
    /// <param name="pkSeed">PK.seed from the SLH-DSA public key</param>
    /// <param name="adrs">"address" containing the tree address of the XMSS tree that signs the FORS key and the key pair address of the WOTS+ key within the XMSS tree that signs the FORS key.</param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use.</param>
    /// <returns>A candidate FORS public key.</returns>
    byte[] ForsPKFromSig(byte[] sigFors, byte[] md, byte[] pkSeed, ForsTreeAdrs adrs, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes);
}
