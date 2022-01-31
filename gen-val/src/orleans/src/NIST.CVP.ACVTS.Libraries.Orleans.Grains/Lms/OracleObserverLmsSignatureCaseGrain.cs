using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Lms;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Lms
{
    public class OracleObserverLmsSignatureCaseGrain : ObservableOracleGrainBase<LmsSignatureResult>,
        IOracleObserverLmsSignatureCaseGrain
    {

        private readonly IHssFactory _hssFactory;
        private readonly IEntropyProvider _entropyProvider;

        private LmsSignatureParameters _param;

        public OracleObserverLmsSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IHssFactory hssFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _hssFactory = hssFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(LmsSignatureParameters param)
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

            var message = _entropyProvider.GetEntropy(1024);

            var keyPair = await hss.GenerateHssKeyPairAsync();

            var signature = await hss.GenerateHssSignatureAsync(message, keyPair, _param.Advance);

            // Notify observers of result
            await Notify(new LmsSignatureResult
            {
                Message = message,
                Signature = signature.Signature,
                SEED = seed,
                I = i,
                PublicKey = keyPair.PublicKey
            });
        }
    }
}
