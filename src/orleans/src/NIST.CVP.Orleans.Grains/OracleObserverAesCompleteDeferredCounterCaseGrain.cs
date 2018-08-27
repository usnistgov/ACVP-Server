using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Ctr;
using NIST.CVP.Orleans.Grains.Interfaces;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleObserverAesCompleteDeferredCounterCaseGrain : ObservableOracleGrainBase<AesResult>, 
        IOracleObserverAesCompleteDeferredCounterCaseGrain
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;
        private readonly ICounterFactory _counterFactory;
        private readonly IEntropyProvider _entropyProvider;

        private CounterParameters<AesParameters> _param;

        public OracleObserverAesCompleteDeferredCounterCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IBlockCipherEngineFactory engineFactory,
            IModeBlockCipherFactory modeFactory,
            ICounterFactory counterFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base (nonOrleansScheduler)
        {
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
            _counterFactory = counterFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }
        
        public async Task<bool> BeginWorkAsync(CounterParameters<AesParameters> param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var fullParam = CounterHelpers.GetDeferredCounterTest(_param, _entropyProvider);
            var direction = BlockCipherDirections.Encrypt;
            if (_param.Parameters.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var engine = _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes);
            var counter = _counterFactory.GetCounter(
                engine, 
                _param.Incremental ? CounterTypes.Additive : CounterTypes.Subtractive, fullParam.Iv
            );
            var cipher = _modeFactory.GetCounterCipher(
                engine, 
                counter
            );

            var blockCipherParams = new CounterModeBlockCipherParameters(direction, fullParam.Iv, fullParam.Key, direction == BlockCipherDirections.Encrypt ? fullParam.PlainText : fullParam.CipherText, null);
  
            var result = cipher.ProcessPayload(blockCipherParams);

            // Notify observers of result
            await Notify(new AesResult
            {
                Key = fullParam.Key,
                Iv = fullParam.Iv,
                PlainText = direction == BlockCipherDirections.Encrypt ? fullParam.PlainText : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? fullParam.CipherText : result.Result
            });
        }
    }
}