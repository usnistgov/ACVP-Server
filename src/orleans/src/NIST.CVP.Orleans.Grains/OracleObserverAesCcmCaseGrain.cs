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

namespace NIST.CVP.Orleans.Grains
{
    public class OracleObserverAesCcmCaseGrain : ObservableOracleGrainBase<AeadResult>, 
        IOracleObserverAesCcmCaseGrain
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IAeadRunner _aeadRunner;
        private readonly IAeadModeBlockCipherFactory _aeadCipherFactory;

        private AeadParameters _param;

        public OracleObserverAesCcmCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IBlockCipherEngineFactory engineFactory,
            IEntropyProviderFactory entropyProviderFactory,
            IAeadRunner aeadRunner,
            IAeadModeBlockCipherFactory aeadCipherFactory
        ) : base (nonOrleansScheduler)
        {
            _engineFactory = engineFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
            _aeadRunner = aeadRunner;
            _aeadCipherFactory = aeadCipherFactory;
        }
        
        public async Task<bool> BeginWorkAsync(AeadParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var fullParams = new AeadResult
            {
                PlainText = _entropyProvider.GetEntropy(_param.DataLength),
                Key = _entropyProvider.GetEntropy(_param.KeyLength),
                Iv = _entropyProvider.GetEntropy(_param.IvLength),
                Aad = _entropyProvider.GetEntropy(_param.AadLength),
            };

            var result = _aeadRunner.DoSimpleAead(
                _aeadCipherFactory.GetAeadCipher(
                    _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), 
                    BlockCipherModesOfOperation.Ccm
                ), 
                fullParams, _param
            );

            // Notify observers of result
            await Notify(result);
        }
    }
}