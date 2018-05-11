using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes.ShiftRegister;

namespace NIST.CVP.Crypto.Symmetric.BlockModes
{
    public class ModeBlockCipherFactory : IModeBlockCipherFactory
    {
        /*
            TODO should different types of instances be broken out as their own enums?
            "standard", "counter", "aead", etc?
        */

        public IModeBlockCipher<SymmetricCipherResult> GetStandardCipher(
            IBlockCipherEngine engine, 
            BlockCipherModesOfOperation modeOfOperation
        )
        {
            switch (modeOfOperation)
            {
                case BlockCipherModesOfOperation.Cbc:
                    return new CbcBlockCipher(engine);
                case BlockCipherModesOfOperation.CbcMac:
                    return new CbcMacBlockCipher(engine);
                case BlockCipherModesOfOperation.CfbBit:
                    return new CfbBlockCipher(engine, new ShiftRegisterStrategyBit(engine));
                case BlockCipherModesOfOperation.CfbByte:
                    return new CfbBlockCipher(engine, new ShiftRegisterStrategyByte(engine));
                case BlockCipherModesOfOperation.CfbBlock:
                    return new CfbBlockCipher(engine, new ShiftRegisterStrategyFullBlock(engine));
                case BlockCipherModesOfOperation.Ctr:
                    throw new ArgumentException($"{modeOfOperation} not a standard mode, use {nameof(GetCounterCipher)} instead");
                case BlockCipherModesOfOperation.Ecb:
                    return new EcbBlockCipher(engine);

                default:
                    throw new ArgumentException(nameof(modeOfOperation));
            }
        }

        public IModeBlockCipher<SymmetricCounterResult> GetCounterCipher(IBlockCipherEngine engine, ICounter counter)
        {
            return new CtrBlockCipher(engine, counter);
        }
    }
}
