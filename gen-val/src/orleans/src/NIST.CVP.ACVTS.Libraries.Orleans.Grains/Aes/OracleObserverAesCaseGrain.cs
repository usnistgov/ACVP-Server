using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Aes;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Aes
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
        ) : base(nonOrleansScheduler)
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

            var direction = _param.Direction.ToLower() == "encrypt" ? BlockCipherDirections.Encrypt : BlockCipherDirections.Decrypt;
            var payload = _entropyProvider.GetEntropy(_param.DataLength);
            var key = _param.KeyLength == 0 ? _param.Key : _entropyProvider.GetEntropy(_param.KeyLength);
            var iv = _entropyProvider.GetEntropy(128);

            var blockCipherParams = new ModeBlockCipherParameters(
                direction,
                iv.GetDeepCopy(),
                key.GetDeepCopy(),
                payload.GetDeepCopy()
            );

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
