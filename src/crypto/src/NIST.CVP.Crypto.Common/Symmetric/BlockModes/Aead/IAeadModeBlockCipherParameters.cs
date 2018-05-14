using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead
{
    /// <summary>
    /// Authenticated Block Cipher mode.
    /// </summary>
    public interface IAeadModeBlockCipherParameters : IModeBlockCipherParameters
    {
        /// <summary>
        /// Additional Authenticated data
        /// </summary>
        BitString AdditionalAuthenticatedData { get; set; }
        /// <summary>
        /// The length of the tag in bits
        /// </summary>
        int TagLength { get; }
        /// <summary>
        /// The tag
        /// </summary>
        BitString Tag { get; set; }
    }
}