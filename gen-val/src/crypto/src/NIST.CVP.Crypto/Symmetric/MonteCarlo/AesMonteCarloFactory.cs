using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using System;

namespace NIST.CVP.Crypto.Symmetric.MonteCarlo
{
    public class AesMonteCarloFactory : IMonteCarloFactoryAes
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;

        public AesMonteCarloFactory(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory)
        {
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
        }

        public IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse> GetInstance(
            BlockCipherModesOfOperation mode
        )
        {
            var keyMaker = new AesMonteCarloKeyMaker();

            switch (mode)
            {
                case BlockCipherModesOfOperation.Ecb:
                    return new MonteCarloAesEcb(_engineFactory, _modeFactory, keyMaker);
                case BlockCipherModesOfOperation.Cbc:
                    return new MonteCarloAesCbc(_engineFactory, _modeFactory, keyMaker);
                case BlockCipherModesOfOperation.CbcCs1:
                case BlockCipherModesOfOperation.CbcCs2:
                case BlockCipherModesOfOperation.CbcCs3:
                    return new MonteCarloAesCbcCts(_engineFactory, _modeFactory, keyMaker, mode);
                case BlockCipherModesOfOperation.CfbBit:
                    return new MonteCarloAesCfb(_engineFactory, _modeFactory, keyMaker, 1, BlockCipherModesOfOperation.CfbBit);
                case BlockCipherModesOfOperation.CfbByte:
                    return new MonteCarloAesCfb(_engineFactory, _modeFactory, keyMaker, 8, BlockCipherModesOfOperation.CfbByte);
                case BlockCipherModesOfOperation.CfbBlock:
                    return new MonteCarloAesCfb(_engineFactory, _modeFactory, keyMaker, 128, BlockCipherModesOfOperation.CfbBlock);
                case BlockCipherModesOfOperation.Ofb:
                    return new MonteCarloAesOfb(_engineFactory, _modeFactory, keyMaker);
                case BlockCipherModesOfOperation.Cbci:
                case BlockCipherModesOfOperation.CbcMac:
                case BlockCipherModesOfOperation.Ccm:
                case BlockCipherModesOfOperation.CfbpBit:
                case BlockCipherModesOfOperation.CfbpByte:
                case BlockCipherModesOfOperation.CfbpBlock:
                case BlockCipherModesOfOperation.Ctr:
                case BlockCipherModesOfOperation.Gcm:
                case BlockCipherModesOfOperation.Ofbi:
                    throw new NotSupportedException();
                default:
                    throw new ArgumentException(nameof(mode));
            }
        }
    }
}