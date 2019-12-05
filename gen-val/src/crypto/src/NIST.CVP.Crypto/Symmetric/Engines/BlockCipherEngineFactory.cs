using System;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.Crypto.Symmetric.Engines
{
    public class BlockCipherEngineFactory : IBlockCipherEngineFactory
    {
        public IBlockCipherEngine GetSymmetricCipherPrimitive(BlockCipherEngines blockCipherEngine)
        {
            switch (blockCipherEngine)
            {
                case BlockCipherEngines.Aes:
                    return new AesEngine();
                case BlockCipherEngines.Tdes:
                    return new TdesEngine();
                default:
                    throw new ArgumentException(nameof(blockCipherEngine));
            }
        }
    }
}