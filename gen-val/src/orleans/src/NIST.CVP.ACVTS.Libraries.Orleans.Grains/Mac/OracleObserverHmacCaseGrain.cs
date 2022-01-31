using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Mac;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Mac
{
    public class OracleObserverHmacCaseGrain : ObservableOracleGrainBase<MacResult>,
        IOracleObserverHmacCaseGrain
    {
        private readonly IHmacFactory _macFactory;
        private readonly IEntropyProvider _entropyProvider;

        private HmacParameters _param;

        public OracleObserverHmacCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IHmacFactory macFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _macFactory = macFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(HmacParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var hmac = _macFactory.GetHmacInstance(new HashFunction(_param.ShaMode, _param.ShaDigestSize));

            var key = _entropyProvider.GetEntropy(_param.KeyLength);
            var msg = _entropyProvider.GetEntropy(_param.MessageLength);

            var mac = hmac.Generate(key, msg, _param.MacLength);
            var result = new MacResult()
            {
                Key = key,
                Message = msg,
                Tag = mac.Mac
            };

            // Notify observers of result
            await Notify(result);
        }
    }
}
