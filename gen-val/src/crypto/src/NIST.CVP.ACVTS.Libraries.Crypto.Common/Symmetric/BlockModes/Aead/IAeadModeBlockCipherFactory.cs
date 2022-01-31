using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.Aead
{
    public interface IAeadModeBlockCipherFactory
    {
        /// <summary>
        /// Retrieve a AEAD symmetric cipher instance
        /// </summary>
        /// <param name="engine">The engine to wrap</param>
        /// <param name="modeOfOperation">The mode to wrap around the engine</param>
        /// <returns>A mode wrapping a symmetric block cipher</returns>
        IAeadModeBlockCipher GetAeadCipher(
            IBlockCipherEngine engine,
            BlockCipherModesOfOperation modeOfOperation
        );
    }
}
