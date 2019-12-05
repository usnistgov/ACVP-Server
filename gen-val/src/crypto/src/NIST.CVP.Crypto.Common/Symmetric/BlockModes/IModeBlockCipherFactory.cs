using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.Crypto.Common.Symmetric.BlockModes
{
    /// <summary>
    /// Provides a means of retrieving a mode of operation wrapping a symmetric block cipher
    /// </summary>
    public interface IModeBlockCipherFactory
    {
        /// <summary>
        /// Retrieve a standard symmetric cipher instance (non AEAD/Counter)
        /// </summary>
        /// <param name="engine">The engine to wrap</param>
        /// <param name="modeOfOperation">The mode to wrap around the engine</param>
        /// <returns>A mode wrapping a symmetric block cipher</returns>
        IModeBlockCipher<SymmetricCipherResult> GetStandardCipher(
            IBlockCipherEngine engine,
            BlockCipherModesOfOperation modeOfOperation
        );

        /// <summary>
        /// Get a counter symmetric cipher instance
        /// </summary>
        /// <param name="engine">The engine to wrap</param>
        /// <param name="counter">The counter implementation to use</param>
        /// <returns>A mode wrapping a symmetric block cipher</returns>
        IModeBlockCipher<SymmetricCounterResult> GetCounterCipher(
            IBlockCipherEngine engine,
            ICounter counter
        );

        ICounterModeBlockCipher GetIvExtractor(IBlockCipherEngine engine);
    }
}