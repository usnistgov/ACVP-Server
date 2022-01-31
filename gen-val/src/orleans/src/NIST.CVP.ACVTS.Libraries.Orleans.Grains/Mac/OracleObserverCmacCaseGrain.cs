using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Mac;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Mac
{
    public class OracleObserverCmacCaseGrain : ObservableOracleGrainBase<MacResult>,
        IOracleObserverCmacCaseGrain
    {
        private const double FAIL_RATIO = 0.25;

        private readonly ICmacFactory _macFactory;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IRandom800_90 _rand;

        private CmacParameters _param;

        public OracleObserverCmacCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ICmacFactory macFactory,
            IEntropyProviderFactory entropyProviderFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _macFactory = macFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(CmacParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var cmac = _macFactory.GetCmacInstance(_param.CmacType);

            BitString key = null;

            if (_param.KeyingOption is default(int))
            {
                key = _entropyProvider.GetEntropy(_param.KeyLength);
            }
            else
            {
                key = TdesHelpers.GenerateTdesKey(_param.KeyingOption);
            }

            var msg = _entropyProvider.GetEntropy(_param.PayloadLength);

            var mac = cmac.Generate(key, msg, _param.MacLength);
            var result = new MacResult()
            {
                Key = key,
                Message = msg,
                Tag = mac.Mac,
                TestPassed = true
            };

            if (_param.CouldFail)
            {
                // Should Fail at certain ratio, 25%
                var upperBound = (int)(1.0 / FAIL_RATIO);
                var shouldFail = _rand.GetRandomInt(0, upperBound) == 0;

                if (shouldFail)
                {
                    result.Tag = _rand.GetDifferentBitStringOfSameSize(result.Tag);
                    result.TestPassed = false;
                }
            }

            // Notify observers of result
            await Notify(result);
        }
    }
}
