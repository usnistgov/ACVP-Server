using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Math;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Dsa;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains.Dsa
{
    public class OracleObserverDsaPqCaseCaseGrain : ObservableOracleGrainBase<DsaDomainParametersResult>, 
        IOracleObserverDsaPqCaseGrain
    {

        private readonly IShaFactory _shaFactory;
        private readonly IPQGeneratorValidatorFactory _pqGenFactory;
        private readonly IEntropyProvider _entropyProvider;

        private DsaDomainParametersParameters _param;

        public OracleObserverDsaPqCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IShaFactory shaFactory,
            IPQGeneratorValidatorFactory pqGenFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base (nonOrleansScheduler)
        {
            _shaFactory = shaFactory;
            _pqGenFactory = pqGenFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }
        
        public async Task<bool> BeginWorkAsync(DsaDomainParametersParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var sha = _shaFactory.GetShaInstance(_param.HashAlg);
            var pqGen = _pqGenFactory.GetGeneratorValidator(_param.PQGenMode, sha, EntropyProviderTypes.Random);

            var result = pqGen.Generate(_param.L, _param.N, _param.N);
            if (!result.Success)
            {
                throw new Exception();
            }

            var domainParams = new DsaDomainParametersResult
            {
                P = result.P,
                Q = result.Q,
                Seed = result.Seed,
                Counter = result.Count
            };

            // If there's no value here, just move on
            if (_param.Disposition == default(string) || _param.Disposition == "none")
            {
                await Notify(domainParams);
                return;
            }

            var friendlyReason = EnumHelpers.GetEnumFromEnumDescription<DsaPQDisposition>(_param.Disposition);
            if (friendlyReason == DsaPQDisposition.ModifyP)
            {
                // Make P not prime
                do
                {
                    domainParams.P = _entropyProvider.GetEntropy(_param.L).ToPositiveBigInteger();

                } while (NumberTheory.MillerRabin(domainParams.P,
                    DSAHelper.GetMillerRabinIterations(_param.L, _param.N)));
            }
            else if (friendlyReason == DsaPQDisposition.ModifyQ)
            {
                // Modify Q so that 0 != (P-1) mod Q
                domainParams.Q = _entropyProvider.GetEntropy(_param.N).ToPositiveBigInteger();
            }
            else if (friendlyReason == DsaPQDisposition.ModifySeed)
            {
                // Modify FirstSeed
                var oldSeed = new BitString(domainParams.Seed.Seed);
                var newSeed = _entropyProvider.GetEntropy(oldSeed.BitLength).ToPositiveBigInteger();

                domainParams.Seed.ModifySeed(newSeed);
            }

            // Notify observers of result
            await Notify(domainParams);
        }
    }
}