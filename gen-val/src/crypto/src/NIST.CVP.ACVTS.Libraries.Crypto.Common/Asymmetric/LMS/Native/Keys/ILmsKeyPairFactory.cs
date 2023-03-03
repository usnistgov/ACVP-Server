using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys
{
    /// <summary>
    /// Exposes a means of creating <see cref="ILmOtsKeyPair"/>s and their parts.
    /// </summary>
    public interface ILmsKeyPairFactory
    {
        /// <summary>
        /// Creates an <see cref="ILmsKeyPair"/> based on the provided parameters. 
        /// </summary>
        /// <param name="lmsMode">The LMS tree mode, which can then be mapped to an attribute, hash function, and tree height.</param>
        /// <param name="lmOtsMode">The LM-OTS mode, which can then be mapped to an attribute, hash function, chunk size.</param>
        /// <param name="i">The 16 byte merkle tree identifier.</param>
        /// <param name="seed">
        ///		The seed used for pseudo-random construction of <see cref="ILmOtsKeyPair"/>.
        ///		For testing in ACVP, *SHALL NOT* be null.
        ///		Must be at least the length of the output of the hash function utilized for <see cref="lmsMode"/>
        /// </param>
        /// <param name="x">The number of layers excluding the root of the computed tree to store.
        ///     x = 0 means do not store any values (just public key kept separate from the tree),
        ///     x = 1 stores nodes 1, 2, 3,
        ///     x = h stores the full tree.
        /// </param>
        ILmsKeyPair GetKeyPair(LmsMode lmsMode, LmOtsMode lmOtsMode, byte[] i, byte[] seed, int x = 0);
    }
}
