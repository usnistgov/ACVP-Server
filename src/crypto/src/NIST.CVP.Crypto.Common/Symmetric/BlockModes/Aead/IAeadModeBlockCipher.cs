namespace NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead
{
    /// <summary>
    /// A Mode of Operation that wraps a block cipher.
    /// Differs from <see cref="IModeBlockCipher{TSymmetricCipherResult}"/> in that
    /// the results can be determined as successful or not when decrypting.
    /// </summary>
    public interface IAeadModeBlockCipher
    {
        /// <summary>
        /// Does the mode support partial blocks?
        /// </summary>
        bool IsPartialBlockAllowed { get; }
        /// <summary>
        /// Process a message using the mode wrapping the symmetric block cipher
        /// </summary>
        /// <param name="param">The parameters for performing the cipher operation</param>
        /// <returns>The result of the crypto function</returns>
        SymmetricCipherAeadResult ProcessPayload(IAeadModeBlockCipherParameters param);
    }
}