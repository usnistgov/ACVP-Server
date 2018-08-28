using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Orleans.Grains.Interfaces;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleObserverDsaKeyCaseCaseGrain : ObservableOracleGrainBase<DsaKeyResult>, 
        IOracleObserverDsaKeyCaseGrain
    {

        private readonly IDsaFfcFactory _dsaFactory;

        private DsaKeyParameters _param;

        public OracleObserverDsaKeyCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IDsaFfcFactory dsaFactory
        ) : base (nonOrleansScheduler)
        {
            _dsaFactory = dsaFactory;
        }
        
        public async Task<bool> BeginWorkAsync(DsaKeyParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d256);
            var dsa = _dsaFactory.GetInstance(hashFunction);

            var result = dsa.GenerateKeyPair(_param.DomainParameters);
            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new DsaKeyResult
            {
                Key = result.KeyPair
            });
        }
    }
}