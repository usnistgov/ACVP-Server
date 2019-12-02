using System;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Ffx;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.Crypto.Symmetric.BlockModes.Ffx
{
    public class FfxModeBlockCipherFactory : IFfxModeBlockCipherFactory
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;
        private readonly IAesFfInternals _aesFfInternals;

        public FfxModeBlockCipherFactory(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory, IAesFfInternals aesFfInternals)
        {
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
            _aesFfInternals = aesFfInternals;
        }
        
        public IFfxModeBlockCipher Get(AlgoMode algoMode)
        {
            switch (algoMode)
            {
                case AlgoMode.AES_FF1_v1_0:
                    return new Ff1BlockCipher(_engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), _modeFactory, _aesFfInternals);
                case AlgoMode.AES_FF3_1_v1_0:
                        return new Ff3_1BlockCipher(_engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), _modeFactory, _aesFfInternals);
                default:
                    throw new ArgumentException($"Invalid {nameof(algoMode)} of {algoMode} passed into factory.");
            }
        }
    }
}