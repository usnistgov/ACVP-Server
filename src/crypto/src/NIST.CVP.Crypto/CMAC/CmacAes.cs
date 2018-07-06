using System;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.CMAC
{
    public class CmacAes : CmacBase
    {
        protected override BitString RbConstant => new BitString("00000000000000000000000000000087");  //TODO this should be 87, not 86
        protected override IModeBlockCipher<SymmetricCipherResult> AlgoMode { get; }
        protected override IBlockCipherEngine Engine { get; }

        public CmacAes(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory)
        {
            Engine = engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes);
            AlgoMode = modeFactory.GetStandardCipher(Engine, BlockCipherModesOfOperation.Ecb);
        }
    }
}
