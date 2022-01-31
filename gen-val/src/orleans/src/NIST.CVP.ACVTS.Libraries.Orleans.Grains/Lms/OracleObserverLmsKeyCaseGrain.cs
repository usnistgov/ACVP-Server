using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Lms;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Lms
{
    public class OracleObserverLmsKeyCaseGrain : ObservableOracleGrainBase<LmsKeyResult>,
        IOracleObserverLmsKeyCaseGrain
    {
        private readonly IHssFactory _hssFactory;

        private readonly IEntropyProvider _entropyProvider;

        private LmsKeyParameters _param;

        public OracleObserverLmsKeyCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IHssFactory hssFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _hssFactory = hssFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(LmsKeyParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var seed = _entropyProvider.GetEntropy(256);

            var i = _entropyProvider.GetEntropy(128);

            var hss = _hssFactory.GetInstance(_param.Layers, _param.LmsTypes, _param.LmotsTypes, EntropyProviderTypes.Testable, seed, i);

            var keyPair = await hss.GenerateHssKeyPairAsync();

            // Notify observers of result
            await Notify(new LmsKeyResult
            {
                SEED = seed,
                I = i,
                KeyPair = keyPair
            });
        }
    }
}
