using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Aes;

namespace NIST.CVP.Orleans.Grains.Aes
{
    public class OracleObserverAesCaseGrain : ObservableOracleGrainBase<AesResult>, 
        IOracleObserverAesCaseGrain
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;
        private readonly IEntropyProvider _entropyProvider;

        private AesParameters _param;

        public OracleObserverAesCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IBlockCipherEngineFactory engineFactory,
            IModeBlockCipherFactory modeFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base (nonOrleansScheduler)
        {
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }
        
        public async Task<bool> BeginWorkAsync(AesParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var cipher = _modeFactory.GetStandardCipher(
                _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), 
                _param.Mode
            );
            var direction = BlockCipherDirections.Encrypt;
            if (_param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _entropyProvider.GetEntropy(_param.DataLength);
            var key = _entropyProvider.GetEntropy(_param.KeyLength);
            var iv = _entropyProvider.GetEntropy(128);

            var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
            var result = cipher.ProcessPayload(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new AesResult
            {
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : result.Result,
                Key = key,
                Iv = iv
            });
        }
    }
}