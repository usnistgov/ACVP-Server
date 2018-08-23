using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Aead;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Enums;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleAesCcmCaseGrain : PollableOracleGrainBase<AeadResult>, IOracleAesCcmCaseGrain
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IAeadRunner _aeadRunner;
        private readonly IAeadModeBlockCipherFactory _aeadCipherFactory;
        private AeadParameters _param;

        public OracleAesCcmCaseGrain(
            LimitedConcurrencyLevelTaskScheduler scheduler,
            IBlockCipherEngineFactory engineFactory,
            IEntropyProviderFactory entropyProviderFactory,
            IAeadRunner aeadRunner,
            IAeadModeBlockCipherFactory aeadCipherFactory
        )
            : base(scheduler)
        {
            _engineFactory = engineFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
            _aeadRunner = aeadRunner;
            _aeadCipherFactory = aeadCipherFactory;
        }

        public Task<bool> BeginWorkAsync(AeadParameters param)
        {
            _param = param;
            BeginGrainWorkAsync();

            return Task.FromResult(true);
        }

        protected override Task DoWorkAsync()
        {
            var fullParams = new AeadResult
            {
                PlainText = _entropyProvider.GetEntropy(_param.DataLength),
                Key = _entropyProvider.GetEntropy(_param.KeyLength),
                Iv = _entropyProvider.GetEntropy(_param.IvLength),
                Aad = _entropyProvider.GetEntropy(_param.AadLength),
            };

            Result = _aeadRunner.DoSimpleAead(
                _aeadCipherFactory.GetAeadCipher(
                    _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), 
                    BlockCipherModesOfOperation.Ccm
                ), 
                fullParams, _param
            );

            State = GrainState.CompletedWork;
            return Task.CompletedTask;
        }
    }
}