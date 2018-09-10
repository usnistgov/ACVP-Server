using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
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
                    return new MonteCarloTdesEcb(_engineFactory, _modeFactory, new TDES_ECB.MonteCarloKeyMaker());
                case BlockCipherModesOfOperation.Cbc:
                    return new MonteCarloTdesCbc(_engineFactory, _modeFactory, new TDES_CBC.MonteCarloKeyMaker());
                case BlockCipherModesOfOperation.CfbBit:
                    return new MonteCarloTdesCfb(_engineFactory, _modeFactory, new TDES_CFB.MonteCarloKeyMaker(), 
                        1, mode);
                case BlockCipherModesOfOperation.CfbByte:
                    return new MonteCarloTdesCfb(_engineFactory, _modeFactory, new TDES_CFB.MonteCarloKeyMaker(),
                        8, mode);
                case BlockCipherModesOfOperation.CfbBlock:
                    return new MonteCarloTdesCfb(_engineFactory, _modeFactory, new TDES_CFB.MonteCarloKeyMaker(),
                        64, mode);
                case BlockCipherModesOfOperation.Ofb:
                    return new MonteCarloTdesOfb(_engineFactory, _modeFactory, new TDES_OFB.MonteCarloKeyMaker());
                
                case BlockCipherModesOfOperation.Cbci:
                    return new MonteCarloTdesCbci(_engineFactory, _modeFactory, new TDES_CBCI.MonteCarloKeyMaker());
                case BlockCipherModesOfOperation.CfbpBit:
                    return new MonteCarloTdesCfbp(_engineFactory, _modeFactory, new TDES_CFBP.MonteCarloKeyMaker(), mode);
                case BlockCipherModesOfOperation.CfbpByte:
                    return new MonteCarloTdesCfbp(_engineFactory, _modeFactory, new TDES_CFBP.MonteCarloKeyMaker(), mode);
                case BlockCipherModesOfOperation.CfbpBlock:
                    return new MonteCarloTdesCfbp(_engineFactory, _modeFactory, new TDES_CFBP.MonteCarloKeyMaker(), mode);
                case BlockCipherModesOfOperation.Ofbi:
                    return new MonteCarloTdesOfbi(_engineFactory, _modeFactory, new TDES_OFBI.MonteCarloKeyMaker());

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