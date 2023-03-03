using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Lms.Native;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Lms
{
    public class OracleObserverLmsKeyCaseGrain : ObservableOracleGrainBase<LmsKeyPairResult>,
        IOracleObserverLmsKeyCaseGrain
    {
        private readonly ILmsKeyPairFactory _lmsKeyPairFactory;
        private readonly IEntropyProvider _entropyProvider;
        private LmsKeyPairParameters _param;

        public OracleObserverLmsKeyCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ILmsKeyPairFactory lmsKeyPairFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _lmsKeyPairFactory = lmsKeyPairFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(LmsKeyPairParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var seed = _entropyProvider.GetEntropy(256);
            var i = _entropyProvider.GetEntropy(128);

            var lmsKey = _lmsKeyPairFactory.GetKeyPair(_param.LmsMode, _param.LmOtsMode, i.ToBytes(), seed.ToBytes());

            // Notify observers of result
            await Notify(new LmsKeyPairResult
            {
                Seed = seed,
                I = i,
                KeyPair = lmsKey
            });
        }
    }
}
