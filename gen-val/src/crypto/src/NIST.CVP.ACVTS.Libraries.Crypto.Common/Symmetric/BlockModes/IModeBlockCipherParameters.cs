using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes
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
        BitString Iv { get; set; }
        /// <summary>
        /// The key
        /// </summary>
        BitString Key { get; set; }
        /// <summary>
        /// The data to encrypt/decrypt
        /// </summary>
        BitString Payload { get; set; }
        /// <summary>
        /// Forward or reverse cipher mode?
        /// </summary>
        bool UseInverseCipherMode { get; }
    }
}
