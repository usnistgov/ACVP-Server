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
    public class OracleObserverBlockCipherDfCaseGrain : ObservableOracleGrainBase<AesResult>, IOracleObserverBlockCipherDfCaseGrain
    {
        private AesParameters _param;
        private readonly IBlockCipherConditioningComponentFactory _factory;
        private readonly IEntropyProvider _entropyProvider;

        public OracleObserverBlockCipherDfCaseGrain(LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler, IBlockCipherConditioningComponentFactory factory, IEntropyProviderFactory entropyProviderFactory) : base(nonOrleansScheduler)
        {
            _factory = factory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(AesParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var blockCipherCC = _factory.GetInstance(_param.KeyLength);
            var message = _entropyProvider.GetEntropy(_param.DataLength);
            var bitsToReturn = _param.KeyLength;

            var result = blockCipherCC.DerivationFunction(message, bitsToReturn);
            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new AesResult
            {
                PlainText = message,
                CipherText = result.Bits
            });
        }
    }
}
