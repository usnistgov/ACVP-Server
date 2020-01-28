using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math.Entropy;
using System;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Orleans.Grains.Interfaces.Hash;

namespace NIST.CVP.Orleans.Grains.Hash
{
    public class OracleObserverSha3CaseGrain : ObservableOracleGrainBase<HashResult>, 
        IOracleObserverSha3CaseGrain
    {
        private readonly ISHA3 _sha;
        private readonly IEntropyProvider _entropyProvider;

        private Sha3Parameters _param;

        public OracleObserverSha3CaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ISHA3 sha,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _sha = sha;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(Sha3Parameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var message = _entropyProvider.GetEntropy(_param.MessageLength);

            var result = _sha.HashMessage(_param.HashFunction, message);

            if (!result.Success)
            {
                throw new Exception();
            }

            await Notify(new HashResult
            {
                Message = message,
                Digest = result.Digest
            });
        }
    }
}
