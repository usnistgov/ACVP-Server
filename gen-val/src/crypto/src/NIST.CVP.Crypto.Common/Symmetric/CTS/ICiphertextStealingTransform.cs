using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.CTS
{
    /// <summary>
    /// Used to apply a ciphertext stealing mode's final block(s) transform.
    ///
    /// https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-38a-add.pdf
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