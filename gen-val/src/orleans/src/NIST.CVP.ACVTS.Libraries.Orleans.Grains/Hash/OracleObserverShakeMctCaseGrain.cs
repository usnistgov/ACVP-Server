using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Hash;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Hash
{
    public class OracleObserverShakeMctCaseGrain : ObservableOracleGrainBase<MctResult<HashResult>>,
        IOracleObserverShakeMctCaseGrain
    {
        private readonly IShaFactory _shaFactory;
        private readonly IEntropyProvider _entropyProvider;

        private ShakeParameters _param;

        public OracleObserverShakeMctCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IShaFactory shaFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _shaFactory = shaFactory;
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
            var shaMct = _shaFactory.GetShaMctInstance(_param.HashFunction);

            var result = shaMct.MctHash(message, false, _param.OutputLengths);

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
