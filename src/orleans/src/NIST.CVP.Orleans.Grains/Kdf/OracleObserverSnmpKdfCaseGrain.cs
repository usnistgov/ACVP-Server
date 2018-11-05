using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KDF.Components.SNMP;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.Orleans.Grains.Kdf
{
    public class OracleObserverSnmpKdfCaseGrain : ObservableOracleGrainBase<SnmpKdfResult>, 
        IOracleObserverSnmpKdfCaseGrain
    {
        private readonly ISnmpFactory _kdfFactory;
        private readonly IRandom800_90 _rand;
        
        private SnmpKdfParameters _param;

        public OracleObserverSnmpKdfCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ISnmpFactory kdfFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _kdfFactory = kdfFactory;
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(SnmpKdfParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var password = _rand.GetRandomAlphaCharacters(_param.PasswordLength);

            var result = _kdfFactory.GetInstance().KeyLocalizationFunction(_param.EngineId, password);
            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new SnmpKdfResult
            {
                Password = password,
                SharedKey = result.SharedKey
            });
        }
    }
}