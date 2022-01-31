using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Dsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Dsa
{
    public class OracleObserverDsaKeyCaseCaseGrain : ObservableOracleGrainBase<DsaKeyResult>,
        IOracleObserverDsaKeyCaseGrain
    {

        private readonly IDsaFfcFactory _dsaFactory;

        private DsaKeyParameters _param;

        public OracleObserverDsaKeyCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IDsaFfcFactory dsaFactory
        ) : base(nonOrleansScheduler)
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
