using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Ecdsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Ecdsa
{
    public class OracleObserverEcdsaDeferredSignatureCaseGrain : ObservableOracleGrainBase<EcdsaSignatureResult>,
        IOracleObserverEcdsaDeferredSignatureCaseGrain
    {

        private readonly IEntropyProvider _entropyProvider;

        private EcdsaSignatureParameters _param;

        public OracleObserverEcdsaDeferredSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
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
            var message = _entropyProvider.GetEntropy(_param.HashAlg.Name.Contains("SHAKE") ? _param.HashAlg.OutputLen : 1024);

            // Notify observers of result
            await Notify(new EcdsaSignatureResult
            {
                Message = message
            });
        }
    }
}
