using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;

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
            switch (mode)
            {
                case BlockCipherModesOfOperation.Ecb:
                    throw new NotImplementedException();
                case BlockCipherModesOfOperation.Cbc:
                    return new MonteCarloAesCbc(_engineFactory, _modeFactory);
                case BlockCipherModesOfOperation.CfbBit:
                    throw new NotImplementedException();
                case BlockCipherModesOfOperation.CfbByte:
                    throw new NotImplementedException();
                case BlockCipherModesOfOperation.CfbBlock:
                    throw new NotImplementedException();
                case BlockCipherModesOfOperation.Ofb:
                    throw new NotImplementedException();
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