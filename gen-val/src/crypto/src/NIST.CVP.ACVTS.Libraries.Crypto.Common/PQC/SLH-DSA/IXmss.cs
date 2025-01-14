using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA;

/// <summary>
/// Abstraction of the XMSS functions defined in section 6 of FIPS 205.
/// </summary>
public interface IXmss
{
    /// <summary>
    ///  Computes the nodes of an XMSS tree. "Each node in an XMSS tree is the root of a subtree, and Algorithm 8 computes
    /// the root of the subtree recursively."
    ///
    /// FIPS 205 Section 6.1, Algorithm 8
    /// </summary>
    /// <param name="skSeed">Secret seed SK.seed from the SLH-DSA private key</param>
    /// <param name="pkSeed">Public seed PK.seed from the SLH-DSA private key</param>
    /// <param name="i">the index value for the node to be calculated</param>
    /// <param name="z">the height value for the node to be calculated</param>
    /// <param name="adrs">"address" containing the layer address and tree address of the XMSS tree within which the node that is being compute belongs.</param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use.</param>
    /// <returns>The n-byte root node associated with <see cref="adrs"/>, <see cref="i"/>, and <see cref="z"/></returns>
    byte[] XmssNode(byte[] skSeed, byte[] pkSeed, int i, int z, WotsHashAdrs adrs, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes);

    /// <summary>
    /// Generates an XMSS signature by first creating an authentication path and then signing M with the appropriate
    /// WOTS+ key.
    /// 
    /// FIPS 205 Section 6.2, Algorithm 9
    /// </summary>
    /// <param name="M">The message for which to produce a signature.</param>
    /// <param name="skSeed">SK.seed from the SLH-DSA private key</param>
    /// <param name="pkSeed">PK.seed from the SLH-DSA private key</param>
    /// <param name="idx">the index of the WOTS+ key within the XMSS tree that will be used to sign the message</param>
    /// <param name="adrs">The address of the XMSS tree w/in the hyper tree that is being used to sign the message.</param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use.</param>
    /// <returns>The computed message <see cref="M"/> signature.</returns>
    byte[] XmssSign(byte[] M, byte[] skSeed, byte[] pkSeed, int idx, WotsHashAdrs adrs,
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes);

    /// <summary>
    /// Computes a candidate XMSS tree root node public key value using the original message, <see cref="M">, and the
    /// provided XMSS signature. Verifying an XMSS signature involves computing a public-key value from
    /// a message and signature value. Does the candidate public key match the known/actual public key?
    ///
    /// FIPS 205 Section 6.3, Algorithm 10
    /// </summary>
    /// <param name="idx">The index of the WOTS+ key within the XMSS tree that was used to sign the message</param>
    /// <param name="sigXmss">The purported signature of <see cref="M"></param>
    /// <param name="M">The message that was signed/whose signature is <see cref="sigXmss"/></param>
    /// <param name="pkSeed">PK.seed from the SLH-DSA public key</param>
    /// <param name="adrs">The address of the XMSS tree w/in the hyper tree that was used to sign the message.</param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use.</param>
    /// <returns>A candidate public key for the root node of the XMSS tree identified by <see cref="adrs"/></returns>
    byte[] XmssPKFromSig(int idx, byte[] sigXmss, byte[] M, byte[] pkSeed, WotsHashAdrs adrs, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes);
}
