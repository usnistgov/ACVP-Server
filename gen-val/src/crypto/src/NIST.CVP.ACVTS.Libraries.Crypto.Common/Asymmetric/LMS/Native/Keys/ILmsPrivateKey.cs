using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys
{
    public interface ILmsPrivateKey
    {
        /// <summary>
        /// The attributes making up the LMS tree - hash, length of output from hash, height of tree
        /// </summary>
        LmsAttribute LmsAttribute { get; }

        /// <summary>
        /// The attributes making up the LMOTS tree. This is needed to reconstruct private keys on the fly
        /// </summary>
        LmOtsAttribute LmOtsAttribute { get; }

        /// <summary>
        /// The 16 byte merkle tree identifier.
        /// </summary>
        byte[] I { get; }

        /// <summary>
        /// The seed used to construct the key.
        /// </summary>
        byte[] Seed { get; }
        
        /// <summary>
        /// The height of the internally stored tree.
        /// </summary>
        int X { get; }
        
        /// <summary>
        /// The internal tree.
        /// </summary>
        byte[][] T { get; }

        /// <summary>
        /// Is the LMS tree exhausted? (Have all "Q" leaf values been used to sign a message?)
        /// </summary>
        bool IsExhausted { get; }

        /// <summary>
        /// Set's the q value that will be pulled with the next call of <see cref="GetQ"/>.
        /// </summary>
        /// <param name="q">The value to set q to.</param>
        void SetQ(int q);

        /// <summary>
        /// Retrieve and allocate the q value from the pool of private keys.
        /// Each q value used for signing messages *exactly* once.
        /// </summary>
        /// <param name="withIncrement">
        ///		When true, the instance value Q will be incremented separate from the returned value.
        ///		Otherwise the instance Q value stays as it was.
        /// </param>
        /// <returns>The q value when one is available, null otherwise.</returns>
        int? GetQ(bool withIncrement = true);
        
        /// <summary>
        /// Determine if the particular node has already been computed and stored by the private key
        /// </summary>
        /// <param name="r">The index of the particular node</param>
        /// <returns></returns>
        bool HasPrecomputedHash(int r);

        /// <summary>
        /// Returns T[i], where T[] is the tree. The 0th index is the public key.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        byte[] GetTreeNodeAtIndex(int i);
    }
}
