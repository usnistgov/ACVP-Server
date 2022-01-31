using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.ConditioningComponents;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Drbg;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Drbg
{
    public class OracleObserverHashDfCaseGrain : ObservableOracleGrainBase<HashResult>, IOracleObserverHashDfCaseGrain
    {
        private ShaWrapperParameters _param;
        private readonly IHashConditioningComponentFactory _factory;
        private readonly IEntropyProvider _entropyProvider;

        public OracleObserverHashDfCaseGrain(LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler, IHashConditioningComponentFactory factory, IEntropyProviderFactory entropyProviderFactory) : base(nonOrleansScheduler)
        {
            _factory = factory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(ShaWrapperParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var hashCC = _factory.GetInstance(_param.HashFunction);
            var message = _entropyProvider.GetEntropy(_param.MessageLength);
            var bitsToReturn = _param.HashFunction.OutputLen;

            var result = hashCC.DerivationFunction(message, bitsToReturn);
            if (!result.Success)
            {
                throw new Exception();
            }


            // Notify observers of result
            await Notify(new HashResult
            {
                Message = message,
                Digest = result.Bits
            });
        }
    }
}
