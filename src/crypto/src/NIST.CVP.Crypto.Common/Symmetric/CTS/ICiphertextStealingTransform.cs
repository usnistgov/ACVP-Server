using NIST.CVP.Crypto.Common.Symmetric.Engines;

namespace NIST.CVP.Crypto.Common.Symmetric.CTS
{
    /// <summary>
    /// Used to apply a ciphertext stealing mode's final block(s) transform.
    /// </summary>
    public interface ICiphertextStealingTransform
    {
        /// <summary>
        /// Swap ciphertext blocks given the ciphertext, cipher engine, and total number of blocks,
        /// </summary>
        /// <param name="outBuffer">The ciphertext to swap.</param>
        /// <param name="engine">The Block Cipher Engine.</param>
        /// <param name="numberOfBlocks">The number of blocks in the ciphertext.</param>
        /// <param name="originalPayloadBitLength">The original payloads length in bits.</param></para>
        /// <returns></returns>
        void Transform(byte[] outBuffer, IBlockCipherEngine engine, int numberOfBlocks, int originalPayloadBitLength);
    }
}