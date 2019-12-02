using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.KDF;
using NIST.CVP.Orleans.Grains.Interfaces.Kdf;
using KdfResult = NIST.CVP.Common.Oracle.ResultTypes.KdfResult;

namespace NIST.CVP.Orleans.Grains.Kdf
{
    public class OracleObserverKdfCompleteDeferredCaseGrain : ObservableOracleGrainBase<KdfResult>, 
        IOracleObserverKdfCompleteDeferredCaseGrain
    {
        private readonly IKdfFactory _kdfFactory;

        private KdfParameters _param;
        private KdfResult _fullParam;

        public OracleObserverKdfCompleteDeferredCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKdfFactory kdfFactory
        ) : base (nonOrleansScheduler)
        {
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
            var kdf = _kdfFactory.GetKdfInstance(_param.Mode, _param.MacMode, _param.CounterLocation, _param.CounterLength);

            var result = kdf.DeriveKey(_fullParam.KeyIn, _fullParam.FixedData, _param.KeyOutLength, _fullParam.Iv, _fullParam.BreakLocation);

            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new KdfResult
            {
                KeyOut = result.DerivedKey
            });
        }
    }
}