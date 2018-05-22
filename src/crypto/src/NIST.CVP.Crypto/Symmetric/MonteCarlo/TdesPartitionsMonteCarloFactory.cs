using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.TDES_CBCI;

namespace NIST.CVP.Crypto.Symmetric.MonteCarlo
{
    public class TdesPartitionsMonteCarloFactory : IMonteCarloFactoryTdesPartitions
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;

        public TdesPartitionsMonteCarloFactory(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory)
        {
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
        }

        public IMonteCarloTester<Common.Symmetric.MCTResult<AlgoArrayResponseWithIvs>, AlgoArrayResponseWithIvs> GetInstance(BlockCipherModesOfOperation mode)
        {
            switch (mode)
            {
                case BlockCipherModesOfOperation.Cbci:
                    return new MonteCarloTdesCbci(_engineFactory, _modeFactory, new MonteCarloKeyMaker());
                case BlockCipherModesOfOperation.CfbpBit:
                    throw new NotImplementedException();
                case BlockCipherModesOfOperation.CfbpByte:
                    throw new NotImplementedException();
                case BlockCipherModesOfOperation.CfbpBlock:
                    throw new NotImplementedException();
                case BlockCipherModesOfOperation.Ofbi:
                    throw new NotImplementedException();
                case BlockCipherModesOfOperation.Ecb:
                case BlockCipherModesOfOperation.Cbc:
                case BlockCipherModesOfOperation.CbcMac:
                case BlockCipherModesOfOperation.Ccm:
                case BlockCipherModesOfOperation.CfbBit:
                case BlockCipherModesOfOperation.CfbByte:
                case BlockCipherModesOfOperation.CfbBlock:
                case BlockCipherModesOfOperation.Ctr:
                case BlockCipherModesOfOperation.Gcm:
                case BlockCipherModesOfOperation.Ofb:
                    throw new NotSupportedException(nameof(mode));
                default:
                    throw new ArgumentException(nameof(mode));
            }
        }
    }
}