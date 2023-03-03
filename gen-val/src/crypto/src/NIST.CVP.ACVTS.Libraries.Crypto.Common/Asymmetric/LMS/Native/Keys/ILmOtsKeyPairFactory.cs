using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys
{
    /// <summary>
    /// Exposes a means of creating <see cref="ILmOtsKeyPair"/>s and their parts.
    /// </summary>
    public interface ILmOtsKeyPairFactory
    {
        /// <summary>
        /// Creates an <see cref="ILmOtsKeyPair"/> based on the provided parameters. 
        /// </summary>
        /// <param name="mode">The mode, which can then be mapped to an attribute and hash function.</param>
        /// <param name="i">The 16 byte merkle tree identifier.</param>
        /// <param name="q">The 32 bit merkle tree leaf indicator.</param>
        /// <param name="seed">
        ///		The seed used for pseudo-random construction of x.
        ///		For testing in ACVP, *SHALL NOT* be null.
        ///		Must be at least the length of the output of the hash function utilized for <see cref="mode"/>
        /// </param>
        /// <returns>The created <see cref="ILmOtsKeyPair"/>.</returns>
        ILmOtsKeyPair GetKeyPair(LmOtsMode mode, byte[] i, byte[] q, byte[] seed);

        /// <summary>
        /// Creates an <see cref="ILmOtsPrivateKey"/> based on the provided parameters. 
        /// </summary>
        /// <param name="lmOtsAttribute">The attributes of the key pair - hash function, chaining function chunks, etc..</param>
        /// <param name="i">The 16 byte merkle tree identifier.</param>
        /// <param name="q">The 32 bit merkle tree leaf indicator.</param>
        /// <param name="seed">
        ///		The seed used for pseudo-random construction of x.
        ///		For testing in ACVP, *SHALL NOT* be null.
        ///		Must be at least the length of the output of the hash function utilized for <see cref="lmOtsattribute"/>
        /// </param>
        /// <returns>The created <see cref="ILmOtsKeyPair"/>.</returns>
        ILmOtsPrivateKey GetPrivateKey(LmOtsAttribute lmOtsAttribute, byte[] i, byte[] q, byte[] seed);
        
        
        // /// <summary>
        // /// Gets a <see cref="ILmOtsPublicKey"/> from a <see cref="ILmOtsPrivateKey"/>.
        // /// </summary>
        // /// <param name="privateKey">The <see cref="ILmOtsPrivateKey"/> to get a <see cref="ILmOtsPublicKey"/> for.</param>
        // /// <returns>The corresponding <see cref="ILmOtsPublicKey"/> to the provided <see cref="ILmOtsPrivateKey"/></returns>
        // Task<ILmOtsPublicKey> GetPublicKey(ILmOtsPrivateKey privateKey);
    }
}
