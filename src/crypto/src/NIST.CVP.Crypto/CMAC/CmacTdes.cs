using System;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.CMAC
{
    public class CmacTdes : CmacBase
    {
        protected override BitString RbConstant => new BitString("000000000000001B");  //TODO this should be 1B, not 1A
        protected override IModeBlockCipher<SymmetricCipherResult> AlgoMode { get; }
        protected override IBlockCipherEngine Engine { get; }

        public CmacTdes(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory)
        {
            Engine = engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes);
            AlgoMode = modeFactory.GetStandardCipher(Engine, BlockCipherModesOfOperation.Ecb);
        }
    }
}