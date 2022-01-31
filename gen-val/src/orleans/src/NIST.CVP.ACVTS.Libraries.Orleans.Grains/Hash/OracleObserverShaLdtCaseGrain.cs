using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Math.LargeBitString;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Hash;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Hash
{
    public class OracleObserverShaLdtCaseGrain : ObservableOracleGrainBase<LargeDataHashResult>,
        IOracleObserverShaLdtCaseGrain
    {
        private readonly IShaFactory _shaFactory;
        private readonly IEntropyProvider _entropyProvider;

        private ShaLargeDataParameters _param;

        public OracleObserverShaLdtCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IShaFactory shaFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _shaFactory = shaFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(ShaLargeDataParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var message = _entropyProvider.GetEntropy(_param.MessageLength);
            var sha = _shaFactory.GetShaInstance(_param.HashFunction);

            var largeMessage = new LargeBitString
            {
                Content = message,
                ExpansionTechnique = _param.ExpansionMode,
                FullLength = _param.FullLength
            };

            var result = sha.HashLargeMessage(largeMessage);

            await Notify(new LargeDataHashResult
            {
                MessageContent = largeMessage,
                Digest = result.Digest
            });
        }
    }
}
