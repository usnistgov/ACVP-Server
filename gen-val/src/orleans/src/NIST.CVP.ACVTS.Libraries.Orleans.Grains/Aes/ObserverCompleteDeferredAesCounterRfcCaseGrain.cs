using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Ctr;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Aes;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Aes
{
    public class ObserverCompleteDeferredAesCounterRfcCaseGrain : ObservableOracleGrainBase<AesResult>,
        IObserverCompleteDeferredAesCounterRfcCaseGrain
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;
        private readonly ICounterFactory _counterFactory;

        private AesWithPayloadParameters _param;

        public ObserverCompleteDeferredAesCounterRfcCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IBlockCipherEngineFactory engineFactory,
            IModeBlockCipherFactory modeFactory,
            ICounterFactory counterFactory
        ) : base(nonOrleansScheduler)
        {
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
            _counterFactory = counterFactory;
        }

        public async Task<bool> BeginWorkAsync(AesWithPayloadParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            try
            {
                var engine = _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes);
                var counter = _counterFactory.GetCounter(
                    engine,
                    CounterTypes.Additive,
                    _param.Iv
                );
                var cipher = _modeFactory.GetCounterCipher(
                    engine,
                    counter
                );

                var blockCipherParams = new CounterModeBlockCipherParameters(
                    _param.Direction,
                    _param.Iv, _param.Key,
                    _param.Payload, null);

                var result = cipher.ProcessPayload(blockCipherParams);

                // Notify observers of result
                await Notify(new AesResult
                {
                    Key = _param.Key,
                    Iv = _param.Iv,
                    PlainText = _param.Payload,
                    CipherText = result.Result
                });
            }
            catch (Exception e)
            {
                await Throw(e);
            }
        }
    }
}
