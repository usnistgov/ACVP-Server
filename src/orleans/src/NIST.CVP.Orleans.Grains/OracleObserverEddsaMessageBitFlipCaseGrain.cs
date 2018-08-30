using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleObserverEddsaMessageBitFlipCaseGrain : ObservableOracleGrainBase<BitString>, 
        IOracleObserverEddsaMessageBitFlipCaseGrain
    {
        private readonly IEntropyProvider _entropyProvider;

        private EddsaMessageParameters _param;

        public OracleObserverEddsaMessageBitFlipCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEntropyProviderFactory entropyProviderFactory
        ) : base (nonOrleansScheduler)
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