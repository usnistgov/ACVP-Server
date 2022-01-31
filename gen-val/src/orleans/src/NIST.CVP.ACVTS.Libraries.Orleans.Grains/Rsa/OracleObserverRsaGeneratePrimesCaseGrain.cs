using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Rsa
{
    public class OracleObserverRsaGeneratePrimesCaseGrain : ObservableOracleGrainBase<RsaPrimeResult>,
        IOracleObserverRsaGeneratePrimesCaseGrain
    {
        private readonly IRsaRunner _rsaRunner;
        private readonly IRandom800_90 _random;

        private IEntropyProvider _entropyProvider;

        private RsaKeyParameters _param;

        public OracleObserverRsaGeneratePrimesCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRsaRunner rsaRunner,
            IEntropyProviderFactory entropyProviderFactory,
            IRandom800_90 random
        ) : base(nonOrleansScheduler)
        {
            _rsaRunner = rsaRunner;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
            _random = random;
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
                // Use a special entropy provider for RandomProbablePrimesWithAuxiliaryProbablePrimes
                // to ensure the first bit is 1 for all bitLen related values
                // (xP1, xP2, xQ1, xQ2).
                if (_param.KeyMode == PrimeGenModes.RandomProbablePrimesWithAuxiliaryProbablePrimes)
                {
                    _entropyProvider = new EntropyProviderLeadingOnes(_random, 1);
                }

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
