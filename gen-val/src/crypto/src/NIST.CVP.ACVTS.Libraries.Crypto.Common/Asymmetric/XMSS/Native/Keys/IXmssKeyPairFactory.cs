using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys
{
    /// <summary>
    /// Exposes a means of creating <see cref="IXmssKeyPair"/>s and their parts.
    /// </summary>
    public interface IXmssKeyPairFactory
    {
        /// <summary>
        /// Creates an <see cref="IXmssKeyPair"/> based on the provided parameters.
        /// </summary>
        /// <param name="xmssMode">The XMSS mode, which can then be mapped to an attribute, hash function, and tree height.</param>
        /// <param name="seed">
        ///		The 3n-byte seed used for pseudo-random construction of the key pair,
        ///		consumed as SK_SEED || SK_PRF || SEED per the reference implementation of
        ///		https://datatracker.ietf.org/doc/html/rfc8391#section-7.
        ///		For testing in ACVP, *SHALL NOT* be null.
        /// </param>
        /// <param name="x">The number of levels excluding the root of the computed tree to store.
        ///     x = 0 means do not store any values (just public key kept separate from the tree),
        ///     x = 1 stores nodes 1, 2, 3,
        ///     x = h stores the full tree.
        /// </param>
        IXmssKeyPair GetKeyPair(XmssMode xmssMode, byte[] seed, int x = 0);
    }
}
