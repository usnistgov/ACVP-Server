namespace NIST.CVP.Crypto.Common.Symmetric.BlockModes
{
    /// <summary>
    /// A Mode of Operation that wraps a block cipher
    /// </summary>
    /// <typeparam name="TSymmetricCipherResult">The result of the cipher</typeparam>
    public interface IModeBlockCipher<out TSymmetricCipherResult>
        where TSymmetricCipherResult : IModeBlockCipherResult
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
        TSymmetricCipherResult ProcessPayload(IModeBlockCipherParameters param);
    }
}