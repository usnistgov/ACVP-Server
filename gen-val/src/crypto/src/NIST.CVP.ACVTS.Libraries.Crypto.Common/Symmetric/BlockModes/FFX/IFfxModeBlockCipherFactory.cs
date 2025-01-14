using NIST.CVP.ACVTS.Libraries.Common;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.FFX
{
    /// <summary>
    /// Retrieve an instance of a <see cref="IFfxModeBlockCipher"/>.
    /// </summary>
    public interface IFfxModeBlockCipherFactory
    {
        /// <summary>
        /// Get an instance of <see cref="IFfxModeBlockCipher"/> based on an <see cref="AlgoMode"/>.
        /// </summary>
        /// <param name="algoMode">The <see cref="AlgoMode"/> to use for grabbing an instance of <see cref="IFfxModeBlockCipher"/>.</param>
        /// <returns>The instance of <see cref="IFfxModeBlockCipher"/>.</returns>
        IFfxModeBlockCipher Get(AlgoMode algoMode);
    }
}
