using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.TDES_CBC;
using AlgoArrayResponse = NIST.CVP.Crypto.Common.Symmetric.TDES.AlgoArrayResponse;

namespace NIST.CVP.Crypto.Symmetric.MonteCarlo
{
    public class TdesMonteCarloFactory : IMonteCarloFactoryTdes
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;

        public TdesMonteCarloFactory(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory)
        {
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
        }

        public IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse> GetInstance(BlockCipherModesOfOperation mode)
        {
            switch (mode)
            {
                case BlockCipherModesOfOperation.Ecb:
                    throw new NotImplementedException();
                case BlockCipherModesOfOperation.Cbc:
                    return new MonteCarloTdesCbc(_engineFactory, _modeFactory, new TDES_CBC.MonteCarloKeyMaker());
                case BlockCipherModesOfOperation.CfbBit:
                    throw new NotImplementedException();
                case BlockCipherModesOfOperation.CfbByte:
                    throw new NotImplementedException();
                case BlockCipherModesOfOperation.CfbBlock:
                    throw new NotImplementedException();
                case BlockCipherModesOfOperation.Ofb:
                    throw new NotImplementedException();
                case BlockCipherModesOfOperation.Ofbi:
                    throw new NotImplementedException();
                case BlockCipherModesOfOperation.Cbci:
                case BlockCipherModesOfOperation.CfbpBit:
                case BlockCipherModesOfOperation.CfbpByte:
                case BlockCipherModesOfOperation.CfbpBlock:
                    throw new NotSupportedException(nameof(mode));
                case BlockCipherModesOfOperation.CbcMac:
                case BlockCipherModesOfOperation.Ccm:
                case BlockCipherModesOfOperation.Ctr:
                case BlockCipherModesOfOperation.Gcm:
                    throw new NotSupportedException(nameof(mode));
                default:
                    throw new ArgumentException(nameof(mode));
            }
        }
    }
}