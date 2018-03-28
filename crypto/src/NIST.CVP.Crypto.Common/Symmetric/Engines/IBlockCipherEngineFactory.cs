using NIST.CVP.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.Crypto.Common.Symmetric.Engines
{
    /// <summary>
    /// Provides a means of retrieving a symmetric cipher engine primitive
    /// </summary>
    public interface IBlockCipherEngineFactory
    {
        /// <summary>
        /// Get the symmetric cipher instance based on the type requested.
        /// </summary>
        /// <param name="blockCipherEngine">The type of symmetric cipher primitive to retrieve</param>
        /// <returns></returns>
        IBlockCipherEngine GetSymmetricCipherPrimitive(BlockCipherEngines blockCipherEngine);
    }
}