using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Crypto.CMAC
{
    public class CmacTdes : CmacBase
    {
        protected override BitString RbConstant => new BitString("000000000000001B");
        protected override IModeBlockCipher<SymmetricCipherResult> AlgoMode { get; }
        protected override IBlockCipherEngine Engine { get; }

        public CmacTdes(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory)
        {
            Engine = engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes);
            AlgoMode = modeFactory.GetStandardCipher(Engine, BlockCipherModesOfOperation.Ecb);
        }
    }
}
