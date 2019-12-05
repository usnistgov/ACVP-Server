using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes.ShiftRegister;
using NIST.CVP.Crypto.Symmetric.CTS;
using System;

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
                case BlockCipherModesOfOperation.Cbci:
                    return new CbciBlockCipher(engine);
                case BlockCipherModesOfOperation.CbcCs1:
                    return new CbcCtsBlockCipher(engine, new CiphertextStealingMode1());
                case BlockCipherModesOfOperation.CbcCs2:
                    return new CbcCtsBlockCipher(engine, new CiphertextStealingMode2());
                case BlockCipherModesOfOperation.CbcCs3:
                    return new CbcCtsBlockCipher(engine, new CiphertextStealingMode3());
                case BlockCipherModesOfOperation.CbcMac:
                    return new CbcMacBlockCipher(engine);
                case BlockCipherModesOfOperation.CfbBit:
                    return new CfbBlockCipher(engine, new ShiftRegisterStrategyBit(engine));
                case BlockCipherModesOfOperation.CfbByte:
                    return new CfbBlockCipher(engine, new ShiftRegisterStrategyByte(engine));
                case BlockCipherModesOfOperation.CfbBlock:
                    return new CfbBlockCipher(engine, new ShiftRegisterStrategyFullBlock(engine));
                case BlockCipherModesOfOperation.CfbpBit:
                    return new CfbpBlockCipher(engine, new ShiftRegisterStrategyBit(engine));
                case BlockCipherModesOfOperation.CfbpByte:
                    return new CfbpBlockCipher(engine, new ShiftRegisterStrategyByte(engine));
                case BlockCipherModesOfOperation.CfbpBlock:
                    return new CfbpBlockCipher(engine, new ShiftRegisterStrategyFullBlock(engine));
                case BlockCipherModesOfOperation.Ctr:
                    throw new ArgumentException($"{modeOfOperation} not a standard mode, use {nameof(GetCounterCipher)} instead");
                case BlockCipherModesOfOperation.Ecb:
                    return new EcbBlockCipher(engine);
                case BlockCipherModesOfOperation.Ofb:
                    return new OfbBlockCipher(engine);
                case BlockCipherModesOfOperation.Ofbi:
                    return new OfbiBlockCipher(engine);
                case BlockCipherModesOfOperation.Xts:
                    return new XtsBlockCipher(engine);

                default:
                    throw new ArgumentException(nameof(modeOfOperation));
            }
        }

        public IModeBlockCipher<SymmetricCounterResult> GetCounterCipher(IBlockCipherEngine engine, ICounter counter)
        {
            return new CtrBlockCipher(engine, counter);
        }

        public ICounterModeBlockCipher GetIvExtractor(IBlockCipherEngine engine)
        {
            return new CtrBlockCipher(engine, null);
        }
    }
}
