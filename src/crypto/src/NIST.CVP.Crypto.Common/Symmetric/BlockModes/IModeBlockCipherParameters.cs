using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.BlockModes
{
    /// <summary>
    /// Parameters used for invoking a symmetric block cipher on a message.
    /// </summary>
    public interface IModeBlockCipherParameters : ICryptoParameters
    {
        /// <summary>
        /// The direction (encryption/decryption)
        /// </summary>
        BlockCipherDirections Direction { get; }
        /// <summary>
        /// The initialization vector
        /// </summary>
        /// <remarks>
        /// not used for ECB
        /// </remarks>
        BitString Iv { get; }
        /// <summary>
        /// The key
        /// </summary>
        BitString Key { get; }
        /// <summary>
        /// The data to encrypt/decrypt
        /// </summary>
        BitString Payload { get; }
        /// <summary>
        /// Forward or reverse cipher mode?
        /// </summary>
        bool UseInverseCipherMode { get; }
    }
}