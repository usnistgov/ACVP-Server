using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX963;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TPM;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kdf
{
    public class OracleObserverTpmKdfCaseGrain : ObservableOracleGrainBase<TpmKdfResult>,
        IOracleObserverTpmKdfCaseGrain
    {
        private readonly ITpmFactory _kdfFactory;
        private readonly IRandom800_90 _rand;

        public OracleObserverTpmKdfCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ITpmFactory kdfFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _kdfFactory = kdfFactory;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync()
        {
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var tpm = _kdfFactory.GetTpm();

            var auth = _rand.GetRandomBitString(160);
            var nonceEven = _rand.GetRandomBitString(160);
            var nonceOdd = _rand.GetRandomBitString(160);

            var result = tpm.DeriveKey(auth, nonceEven, nonceOdd);

            // Notify observers of result
            await Notify(new TpmKdfResult
            {
                Auth = auth,
                NonceEven = nonceEven,
                NonceOdd = nonceOdd,
                SKey = result.SKey
            });
        }
    }
}
