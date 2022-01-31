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
    public class OracleObserverSha3CaseGrain : ObservableOracleGrainBase<HashResult>,
        IOracleObserverSha3CaseGrain
    {
        private readonly IShaFactory _shaFactory;
        private readonly IEntropyProvider _entropyProvider;

        private ShaParameters _param;

        public OracleObserverSha3CaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IShaFactory shaFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _shaFactory = shaFactory;
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

            var sha = _shaFactory.GetShaInstance(_param.HashFunction);
            var result = sha.HashMessage(message, _param.OutputLength);        // Needs OutputLen for SHAKE

            if (!result.Success)
            {
                throw new Exception(result.ErrorMessage);
            }

            await Notify(new HashResult
            {
                Message = message,
                Digest = result.Digest
            });
        }
    }
}
