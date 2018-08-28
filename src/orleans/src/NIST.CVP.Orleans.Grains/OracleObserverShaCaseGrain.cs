using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleObserverShaCaseGrain : ObservableOracleGrainBase<HashResult>, IOracleObserverShaCaseGrain
    {
        private readonly ISHA _sha;
        private readonly IEntropyProvider _entropyProvider;

        private ShaParameters _param;

        public OracleObserverShaCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ISHA sha,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _sha = sha;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(ShaParameters param)
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
