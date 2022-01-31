using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines
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
