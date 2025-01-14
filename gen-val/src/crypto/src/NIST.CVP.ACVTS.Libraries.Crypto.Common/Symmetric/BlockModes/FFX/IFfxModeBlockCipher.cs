namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.FFX
{
    /// <summary>
    /// A Mode of Operation that wraps a block cipher.
    /// Differs from <see cref="IModeBlockCipher{TSymmetricCipherResult}"/> in that
    /// the results utilize a radix/base for understanding/representing numbers.
    /// </summary>
    public interface IFfxModeBlockCipher
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
        SymmetricCipherResult ProcessPayload(IFfxModeBlockCipherParameters param);
    }
}
