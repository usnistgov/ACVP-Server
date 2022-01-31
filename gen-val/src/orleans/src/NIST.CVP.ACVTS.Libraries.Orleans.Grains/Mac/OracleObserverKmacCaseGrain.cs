using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.KMAC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Mac;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Mac
{
    public class OracleObserverKmacCaseGrain : ObservableOracleGrainBase<KmacResult>,
        IOracleObserverKmacCaseGrain
    {
        private const double FAIL_RATIO = 0.5;

        private readonly IKmacFactory _macFactory;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IRandom800_90 _rand;

        private KmacParameters _param;

        public OracleObserverKmacCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKmacFactory macFactory,
            IEntropyProviderFactory entropyProviderFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _macFactory = macFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(KmacParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var kmac = _macFactory.GetKmacInstance(_param.DigestSize * 2, _param.XOF);

            var key = _entropyProvider.GetEntropy(_param.KeyLength);
            var msg = _entropyProvider.GetEntropy(_param.MessageLength);

            BitString customizationHex = null;
            string customization = string.Empty;
            Crypto.Common.MAC.MacResult mac = null;
            if (_param.HexCustomization)
            {
                if (_param.CouldFail)
                {
                    customizationHex = _entropyProvider.GetEntropy(_rand.GetRandomInt(0, 11) * BitString.BITSINBYTE); // only for mvt
                }
                else
                {
                    customizationHex = _entropyProvider.GetEntropy(_param.CustomizationLength * BitString.BITSINBYTE);
                }

                mac = kmac.Generate(key, msg, customizationHex, _param.MacLength);
            }
            else
            {
                if (_param.CouldFail)
                {
                    customization = _rand.GetRandomString(_rand.GetRandomInt(0, 11)); // only for mvt
                }
                else
                {
                    customization = _rand.GetRandomString(_param.CustomizationLength);
                }

                mac = kmac.Generate(key, msg, customization, _param.MacLength);
            }

            var result = new KmacResult()
            {
                Key = key,
                Message = msg,
                Tag = mac.Mac,
                Customization = customization,
                CustomizationHex = customizationHex
            };

            if (_param.CouldFail)
            {
                // Should Fail at certain ratio, 50%
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
