using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Aes;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Aes
{
    public class OracleObserverAesWithPayloadCaseGrain : ObservableOracleGrainBase<AesResult>,
        IOracleObserverAesWithPayloadCaseGrain
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;
        private readonly IEntropyProvider _entropyProvider;

        private AesWithPayloadParameters _param;

        public OracleObserverAesWithPayloadCaseGrain(
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

        public async Task<bool> BeginWorkAsync(AesWithPayloadParameters param)
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

            var blockCipherParams = new ModeBlockCipherParameters(
                _param.Direction,
                _param.Iv.GetDeepCopy(),
                _param.Key.GetDeepCopy(),
                _param.Payload.GetDeepCopy()
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
                PlainText = _param.Direction == BlockCipherDirections.Encrypt ? _param.Payload : result.Result,
                CipherText = _param.Direction == BlockCipherDirections.Decrypt ? _param.Payload : result.Result,
                Key = _param.Key,
                Iv = _param.Iv
            });
        }
    }
}
