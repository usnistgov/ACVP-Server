using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kdf
{
    public class OracleObserverKdfDeferredCaseGrain : ObservableOracleGrainBase<KdfResult>,
        IOracleObserverKdfDeferredCaseGrain
    {
        private readonly IRandom800_90 _rand;

        private KdfParameters _param;

        public OracleObserverKdfDeferredCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(KdfParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            // Notify observers of result
            await Notify(new KdfResult()
            {
                KeyIn = _rand.GetRandomBitString(_param.KeyInLength),
                Iv = _rand.GetRandomBitString(_param.ZeroLengthIv ? 0 : _param.IvLength),
                FixedData = _rand.GetRandomBitString(128),
                BreakLocation = _rand.GetRandomInt(1, 128)
            });
        }
    }
}
