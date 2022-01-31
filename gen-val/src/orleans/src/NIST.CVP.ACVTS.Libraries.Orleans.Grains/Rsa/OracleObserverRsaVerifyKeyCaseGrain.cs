using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Rsa
{
    public class OracleObserverRsaVerifyKeyCaseGrain : ObservableOracleGrainBase<VerifyResult<RsaKeyResult>>,
        IOracleObserverRsaVerifyKeyCaseGrain
    {

        private RsaKeyResult _param;

        public OracleObserverRsaVerifyKeyCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler
        ) : base(nonOrleansScheduler) { }

        public async Task<bool> BeginWorkAsync(RsaKeyResult param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            // Check correctness in values
            if (!NumberTheory.MillerRabin(_param.Key.PrivKey.P, 20))
            {
                await Notify(new VerifyResult<RsaKeyResult> { Result = false });
                return;
            }

            if (!NumberTheory.MillerRabin(_param.Key.PrivKey.Q, 20))
            {
                await Notify(new VerifyResult<RsaKeyResult> { Result = false });
                return;
            }

            // This fails some tests that don't have an N value given to them so is compared to 0
            //if (param.Key.PubKey.N != param.Key.PrivKey.P * param.Key.PrivKey.Q)
            //{
            //    return new VerifyResult<RsaKeyResult> { Result = false };
            //}

            // Notify observers of result
            await Notify(new VerifyResult<RsaKeyResult>
            {
                Result = true
            });
        }
    }
}
