using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Tdes;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Tdes
{
    public class OracleObserverTdesCounterExtractIvsCaseGrain : ObservableOracleGrainBase<CounterResult>,
        IOracleObserverTdesCounterExtractIvsCaseGrain
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;

        private TdesParameters _param;
        private TdesResult _fullParam;

        public OracleObserverTdesCounterExtractIvsCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IBlockCipherEngineFactory engineFactory,
            IModeBlockCipherFactory modeFactory
        ) : base(nonOrleansScheduler)
        {
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
        }

        public async Task<bool> BeginWorkAsync(TdesParameters param, TdesResult fullParam)
        {
            _param = param;
            _fullParam = fullParam;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var cipher = _modeFactory.GetIvExtractor(
                _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes)
            );
            var direction = BlockCipherDirections.Encrypt;
            if (_param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = direction == BlockCipherDirections.Encrypt ? _fullParam.PlainText : _fullParam.CipherText;
            var result = direction == BlockCipherDirections.Encrypt ? _fullParam.CipherText : _fullParam.PlainText;

            var counterCipherParams = new CounterModeBlockCipherParameters(direction, _fullParam.Iv, _fullParam.Key, payload, result);

            var extractedIvs = cipher.ExtractIvs(counterCipherParams);

            if (!extractedIvs.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new CounterResult
            {
                PlainText = direction == BlockCipherDirections.Encrypt ? null : extractedIvs.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? null : extractedIvs.Result,
                IVs = extractedIvs.IVs
            });
        }
    }
}
