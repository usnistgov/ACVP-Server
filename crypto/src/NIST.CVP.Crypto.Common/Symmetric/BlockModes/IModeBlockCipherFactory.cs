using NIST.CVP.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.Crypto.Common.Symmetric.BlockModes
{
    /// <summary>
    /// Provides a means of retrieving a mode of operation wrapping a symmetric block cipher
    /// </summary>
    public interface IModeBlockCipherFactory
    {
        /// <summary>
        /// Retrieve a standard symmetric cipher instance (non AEAD)
        /// </summary>
        /// <typeparam name="TSymmetricCipherResult">The Mode wrapping a symmetric cipher</typeparam>
        /// <param name="engine">The engine type to retrieve</param>
        /// <param name="modeOfOperation">The mode to wrap around the engine</param>
        /// <returns>A mode wrapping a symmetric block cipher</returns>
        IModeBlockCipher<TSymmetricCipherResult> GetStandardCipher<TSymmetricCipherResult>(
            BlockCipherEngines engine, 
            BlockCipherModesOfOperation modeOfOperation
        )
            where TSymmetricCipherResult : IModeBlockCipherResult;
    }
}