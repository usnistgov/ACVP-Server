using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.Orleans.Grains.Kdf
{
    public class OracleObserverKdfDeferredCaseGrain : ObservableOracleGrainBase<KdfResult>, 
        IOracleObserverKdfDeferredCaseGrain
    {
        private readonly IRandom800_90 _rand;
        
        private KdfParameters _param;

        public OracleObserverKdfDeferredCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
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
                KeyIn = _rand.GetRandomBitString(128),
                Iv = _rand.GetRandomBitString(_param.ZeroLengthIv ? 0 : 128),
                FixedData = _rand.GetRandomBitString(128),
                BreakLocation = _rand.GetRandomInt(1, 128)
            });
        }
    }
}