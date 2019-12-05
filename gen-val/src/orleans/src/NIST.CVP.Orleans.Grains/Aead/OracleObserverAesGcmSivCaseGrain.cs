using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Aead;

namespace NIST.CVP.Orleans.Grains.Aead
{
    public class OracleObserverAesGcmSivCaseGrain : ObservableOracleGrainBase<AeadResult>,
        IOracleObserverAesGcmSivCaseGrain
    {
        private const double FAIL_RATE = 0.25;
        private const int NONCE_LENGTH = 96;
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IAeadRunner _aeadRunner;
        private readonly IAeadModeBlockCipherFactory _aeadCipherFactory;
        private readonly IRandom800_90 _rand;

        private AeadParameters _param;

        public OracleObserverAesGcmSivCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IBlockCipherEngineFactory engineFactory,
            IEntropyProviderFactory entropyProviderFactory,
            IAeadRunner aeadRunner,
            IAeadModeBlockCipherFactory aeadCipherFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _engineFactory = engineFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
            _aeadRunner = aeadRunner;
            _aeadCipherFactory = aeadCipherFactory;
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(AeadParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected async override Task DoWorkAsync()
        {
            var fullParams = new AeadResult
            {
                PlainText = _entropyProvider.GetEntropy(_param.PayloadLength),
                Key = _entropyProvider.GetEntropy(_param.KeyLength),
                Iv = _entropyProvider.GetEntropy(NONCE_LENGTH),
                Aad = _entropyProvider.GetEntropy(_param.AadLength),
            };

            var result = _aeadRunner.DoSimpleAead(
                _aeadCipherFactory.GetAeadCipher(
                    _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), 
                    BlockCipherModesOfOperation.GcmSiv
                ), 
                fullParams, _param
            );

            if (_param.CouldFail)
            {
                // Should Fail at certain ratio, 25%
                var upperBound = (int)(1.0 / FAIL_RATE);
                var shouldFail = _rand.GetRandomInt(0, upperBound) == 0;

                if (shouldFail)
                {
                    result.CipherText = _rand.GetDifferentBitStringOfSameSize(result.CipherText);
                }
            }

            await Notify(result);
        }
    }
}
