using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kdf;
using KdfResult = NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.KdfResult;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kdf
{
    public class OracleObserverKdfCompleteDeferredCaseGrain : ObservableOracleGrainBase<KdfResult>,
        IOracleObserverKdfCompleteDeferredCaseGrain
    {
        private readonly ILogger<OracleObserverKdfCompleteDeferredCaseGrain> _logger;
        private readonly IKdfFactory _kdfFactory;

        private KdfParameters _param;
        private KdfResult _fullParam;

        public OracleObserverKdfCompleteDeferredCaseGrain(
            ILogger<OracleObserverKdfCompleteDeferredCaseGrain> logger,
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKdfFactory kdfFactory) : base(nonOrleansScheduler)
        {
            _logger = logger;
            _kdfFactory = kdfFactory;
        }

        public async Task<bool> BeginWorkAsync(KdfParameters param, KdfResult fullParam)
        {
            _param = param;
            _fullParam = fullParam;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            try
            {
                var kdf = _kdfFactory.GetKdfInstance(_param.Mode, _param.MacMode, _param.CounterLocation, _param.CounterLength);

                var result = kdf.DeriveKey(_fullParam.KeyIn, _fullParam.FixedData, _param.KeyOutLength, _fullParam.Iv, _fullParam.BreakLocation);

                if (!result.Success)
                {
                    await Throw(new Exception(result.ErrorMessage));
                }

                // Notify observers of result
                await Notify(new KdfResult
                {
                    KeyOut = result.DerivedKey
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed completing crypto for KDF");
                await Throw(e);
            }
        }
    }
}
