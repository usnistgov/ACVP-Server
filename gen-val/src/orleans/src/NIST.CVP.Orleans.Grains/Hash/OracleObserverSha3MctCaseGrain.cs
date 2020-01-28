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
    public class OracleObserverSha3MctCaseGrain : ObservableOracleGrainBase<MctResult<HashResult>>, 
        IOracleObserverSha3MctCaseGrain
    {
        private readonly ISHA3_MCT _sha;
        private readonly IEntropyProvider _entropyProvider;

        private Sha3Parameters _param;

        public OracleObserverSha3MctCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ISHA3_MCT sha,
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

            // TODO isSample up in here?
            var result = _sha.MCTHash(_param.HashFunction, message, null, false);

            if (!result.Success)
            {
                throw new Exception();
            }

            await Notify(new MctResult<HashResult>
            {
                Seed = new HashResult { Message = message },
                Results = result.Response.ConvertAll(element =>
                    new HashResult { Message = element.Message, Digest = element.Digest })
            });
        }
    }
}
