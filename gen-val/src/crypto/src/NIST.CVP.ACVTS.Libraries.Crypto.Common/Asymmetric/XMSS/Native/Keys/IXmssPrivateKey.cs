using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys
{
    public interface IXmssPrivateKey
    {
        /// <summary>
        /// The attributes making up the XMSS tree - hash, length of output from hash, height of tree
        /// </summary>
        XmssAttribute XmssAttribute { get; }

        /// <summary>
        /// The n-byte secret seed the WOTS+ chain secrets are derived from.
        /// </summary>
        byte[] SkSeed { get; }

        /// <summary>
        /// The n-byte secret PRF key the per-signature message randomizer is derived from.
        /// </summary>
        byte[] SkPrf { get; }

        /// <summary>
        /// The n-byte public seed used for hash keys and bitmasks.
        /// </summary>
        byte[] PubSeed { get; }

        /// <summary>
        /// The n-byte root node of the tree; an input to the message hash when signing.
        /// </summary>
        byte[] Root { get; }

        /// <summary>
        /// The number of levels below the root of the internally stored tree.
        /// </summary>
        int X { get; }

        /// <summary>
        /// The internal tree.
        /// </summary>
        byte[][] T { get; }

        /// <summary>
        /// Is the XMSS tree exhausted? (Have all leaf index values been used to sign a message?)
        /// </summary>
        bool IsExhausted { get; }

        /// <summary>
        /// Sets the idx value that will be pulled with the next call of <see cref="GetIdx"/>.
        /// </summary>
        /// <param name="idx">The value to set idx to.</param>
        void SetIdx(int idx);

        /// <summary>
        /// Get the current leaf index for signing.
        /// </summary>
        /// <param name="withIncrement">
        ///		When true, increments the internally tracked index, such that leaf indices are not reused.
        /// </param>
        /// <returns>The idx value when one is available, null otherwise.</returns>
        int? GetIdx(bool withIncrement = true);

        /// <summary>
        /// Does the private key contain a precomputed tree node at index <see cref="r"/>?
        /// </summary>
        /// <param name="r">The one-indexed tree node index, where index 1 is the root.</param>
        /// <returns>True when the node is available from the precomputed tree.</returns>
        bool HasPrecomputedHash(int r);

        /// <summary>
        /// Get the precomputed tree node at index <see cref="i"/>.
        /// </summary>
        /// <param name="i">The one-indexed tree node index, where index 1 is the root.</param>
        /// <returns>The n-byte node value.</returns>
        byte[] GetTreeNodeAtIndex(int i);
    }
}
