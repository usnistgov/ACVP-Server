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
    public class OracleObserverShakeMctCaseGrain : ObservableOracleGrainBase<MctResult<HashResult>>, 
        IOracleObserverShakeMctCaseGrain
    {
        private readonly ISHAKE_MCT _shake;
        private readonly IEntropyProvider _entropyProvider;

        private ShakeParameters _param;

        public OracleObserverShakeMctCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ISHAKE_MCT shake,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _shake = shake;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(ShakeParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var message = _entropyProvider.GetEntropy(_param.MessageLength);

            // TODO isSample up in here?
            var result = _shake.MCTHash(_param.HashFunction, message, _param.OutputLengths, false);

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
