using System;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Dsa;

namespace NIST.CVP.Orleans.Grains.Dsa
{
    public class OracleObserverDsaGCaseGrain : ObservableOracleGrainBase<DsaDomainParametersResult>, 
        IOracleObserverDsaGCaseGrain
    {

        private readonly IShaFactory _shaFactory;
        private readonly IGGeneratorValidatorFactory _gGenFactory;
        private readonly IEntropyProvider _entropyProvider;

        private DsaDomainParametersParameters _param;
        private DsaDomainParametersResult _pqParam;

        public OracleObserverDsaGCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IShaFactory shaFactory,
            IGGeneratorValidatorFactory gGenFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base (nonOrleansScheduler)
        {
            _shaFactory = shaFactory;
            _gGenFactory = gGenFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }
        
        public async Task<bool> BeginWorkAsync(DsaDomainParametersParameters param, DsaDomainParametersResult pqParam)
        {
            _param = param;
            _pqParam = pqParam;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            // Make sure index is not "0000 0000"
            BitString index;
            do
            {
                index = _entropyProvider.GetEntropy(8);
            } while (index.Equals(BitString.Zeroes(8)));

            var sha = _shaFactory.GetShaInstance(_param.HashAlg);
            var gGen = _gGenFactory.GetGeneratorValidator(_param.GGenMode, sha);

            var result = gGen.Generate(_pqParam.P, _pqParam.Q, _pqParam.Seed, index);
            if (!result.Success)
            {
                throw new Exception();
            }

            var domainParams = new DsaDomainParametersResult
            {
                G = result.G,
                H = result.H,
                Index = index
            };

            if (_param.Disposition == default(string) || _param.Disposition == "none")
            {
                await Notify(domainParams);
                return;
            }

            // Modify g
            var friendlyReason = EnumHelpers.GetEnumFromEnumDescription<DsaGDisposition>(_param.Disposition);
            if (friendlyReason == DsaGDisposition.ModifyG)
            {
                do
                {
                    domainParams.G = _entropyProvider.GetEntropy(_param.L);

                } while (BigInteger.ModPow(domainParams.G.ToPositiveBigInteger(), _pqParam.Q.ToPositiveBigInteger(), _pqParam.P.ToPositiveBigInteger()) == 1);
            }

            // Notify observers of result
            await Notify(domainParams);
        }
    }
}