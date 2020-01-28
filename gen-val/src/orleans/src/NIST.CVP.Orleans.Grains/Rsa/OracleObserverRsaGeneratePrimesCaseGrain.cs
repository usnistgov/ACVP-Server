using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.Orleans.Grains.Rsa
{
    public class OracleObserverRsaGeneratePrimesCaseGrain : ObservableOracleGrainBase<RsaPrimeResult>, 
        IOracleObserverRsaGeneratePrimesCaseGrain
    {
        private readonly IRsaRunner _rsaRunner;
        private readonly IEntropyProvider _entropyProvider;
        
        private RsaKeyParameters _param;

        public OracleObserverRsaGeneratePrimesCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRsaRunner rsaRunner,
            IEntropyProviderFactory entropyProviderFactory
        ) : base (nonOrleansScheduler)
        {
            _rsaRunner = rsaRunner;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }
        
        public async Task<bool> BeginWorkAsync(RsaKeyParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            try
            {
                var result = _rsaRunner.GeneratePrimes(_param, _entropyProvider);

                // Notify observers of result
                await Notify(result);
            }
            catch (Exception e)
            {
                await Throw(e);
            }
        }
    }
}