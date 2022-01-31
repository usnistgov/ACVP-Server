using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Eddsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Eddsa
{
    public class OracleObserverEddsaMessageBitFlipCaseGrain : ObservableOracleGrainBase<BitString>,
        IOracleObserverEddsaMessageBitFlipCaseGrain
    {
        private readonly IEntropyProvider _entropyProvider;

        private EddsaMessageParameters _param;

        public OracleObserverEddsaMessageBitFlipCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(EddsaMessageParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            // Notify observers of result
            await Notify(_entropyProvider.GetEntropy(_param.IsSample ? 32 : 1024));
        }
    }
}
