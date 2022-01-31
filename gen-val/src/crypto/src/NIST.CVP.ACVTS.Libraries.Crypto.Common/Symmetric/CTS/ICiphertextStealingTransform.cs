using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.CTS
{
    /// <summary>
    /// Used to apply a ciphertext stealing mode's final block(s) transform.
    ///
    /// https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-38a-add.pdf
    ///
    /// Note that the intention of this interface is to "perform encryption/decryption as if running in CS3 mode".
    /// For encrypt: transforming the ciphertext to the appropriate mode for return (either CS1, CS2, or CS3)
    /// For decrypt: transform the mode passed in into a CS3 mode for decryption.
    /// </summary>
    public interface ICiphertextStealingTransform
    {
        /// <summary>
        /// Transforms the ciphertext as per the CS mode
        /// </summary>
        /// <param name="ciphertext">The ciphertext to transform.</param>
        /// <param name="engine">The Block Cipher Engine.</param>
        /// <param name="numberOfBlocks">The number of blocks in the ciphertext.</param>
        /// <param name="originalPayloadBitLength">The original payloads length in bits.</param>
        /// <returns></returns>
        void TransformCiphertext(byte[] ciphertext, IBlockCipherEngine engine, int numberOfBlocks, int originalPayloadBitLength);

        /// <summary>
        /// Performs the decryption full final payload block and the appending of a subset of its bits to itself.
        /// </summary>
        /// <param name="payload">The payload to transform.</param>
        /// <param name="engine">The Block Cipher Engine.</param>
        /// <param name="numberOfBlocks">The number of blocks in the ciphertext.</param>
        /// <param name="originalPayloadBitLength">The original payloads length in bits.</param>
        byte[] HandleFinalFullPayloadBlockDecryption(BitString payload, IBlockCipherEngine engine, int numberOfBlocks, int originalPayloadBitLength);
    }
}
