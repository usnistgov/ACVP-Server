using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SNMP;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kdf
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
        ) : base(nonOrleansScheduler)
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
            // passwordLen comes in as bits, but we want bytes
            var password = _rand.GetRandomAlphaCharacters(_param.PasswordLength / 8);

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
