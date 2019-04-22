namespace NIST.CVP.Crypto.Common.Symmetric.CTS
{
    public interface ICiphertextStealingTransformFactory
    {
        ICiphertextStealingTransform Get(CiphertextStealingMode mode);
    }

    /// <summary>
    /// The differing modes of ciphertext stealing
    /// </summary>
    public enum CiphertextStealingMode
    {
        /// <summary>
        /// The ciphertext is not switched, returned as is.
        /// </summary>
        CS1,
        /// <summary>
        /// Switches the last two blocks of ciphertext conditionally. The blocks are switched only when the last block is not a completely block.
        /// </summary>
        CS2,
        /// <summary>
        /// Always switches last two blocks of ciphertext
        /// </summary>
        CS3
    }
}