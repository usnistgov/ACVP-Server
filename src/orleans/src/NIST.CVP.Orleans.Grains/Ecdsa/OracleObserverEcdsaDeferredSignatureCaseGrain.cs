using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Ecdsa;

namespace NIST.CVP.Orleans.Grains.Ecdsa
{
    public class OracleObserverEcdsaDeferredSignatureCaseGrain : ObservableOracleGrainBase<EcdsaSignatureResult>, 
        IOracleObserverEcdsaDeferredSignatureCaseGrain
    {

        private readonly IEntropyProvider _entropyProvider;

        private EcdsaSignatureParameters _param;

        public OracleObserverEcdsaDeferredSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEntropyProviderFactory entropyProviderFactory
        ) : base (nonOrleansScheduler)
        {
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }
        
        public async Task<bool> BeginWorkAsync(EcdsaSignatureParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var message = _entropyProvider.GetEntropy(_param.PreHashedMessage ? _param.HashAlg.OutputLen : 1024);

            // Notify observers of result
            await Notify(new EcdsaSignatureResult
            {
                Message = message
            });
        }
    }
}