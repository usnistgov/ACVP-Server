using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Eddsa;

namespace NIST.CVP.Orleans.Grains.Eddsa
{
    public class OracleObserverEddsaDeferredSignatureCaseGrain : ObservableOracleGrainBase<EddsaSignatureResult>, 
        IOracleObserverEddsaDeferredSignatureCaseGrain
    {

        private readonly IEntropyProvider _entropyProvider;
        private readonly IRandom800_90 _rand;

        private EddsaSignatureParameters _param;

        public OracleObserverEddsaDeferredSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEntropyProviderFactory entropyProviderFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(EddsaSignatureParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var noContext = _param.Curve == Curve.Ed25519 && !_param.PreHash;

            var message = _entropyProvider.GetEntropy(1024);

            var context = noContext ? new BitString("") : _entropyProvider.GetEntropy(_rand.GetRandomInt(0, 255) * 8);

            // Notify observers of result
            await Notify(new EddsaSignatureResult
            {
                Message = message,
                Context = context
            });
        }
    }
}