using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xmss;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Xmss
{
    public class OracleObserverXmssKeyCaseGrain : ObservableOracleGrainBase<XmssKeyPairResult>,
        IOracleObserverXmssKeyCaseGrain
    {
        /// <summary>
        /// The number of tree levels below the root stored in generated private keys, capping the
        /// per-signature recomputation cost for the taller trees while keeping the serialized
        /// key size modest.
        /// </summary>
        private const int StoredTreeLevels = 10;

        private readonly IXmssKeyPairFactory _xmssKeyPairFactory;
        private readonly IEntropyProvider _entropyProvider;
        private XmssKeyPairParameters _param;

        public OracleObserverXmssKeyCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IXmssKeyPairFactory xmssKeyPairFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _xmssKeyPairFactory = xmssKeyPairFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(XmssKeyPairParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var attribute = AttributesHelper.GetXmssAttribute(_param.XmssMode);

            // The seed is consumed as SK_SEED || SK_PRF || SEED, 3n bytes.
            var seed = _entropyProvider.GetEntropy(3 * attribute.N * 8);

            var xmssKey = _xmssKeyPairFactory.GetKeyPair(_param.XmssMode, seed.ToBytes(),
                System.Math.Min(attribute.H, StoredTreeLevels));

            // Notify observers of result
            await Notify(new XmssKeyPairResult
            {
                Seed = seed,
                KeyPair = xmssKey
            });
        }
    }
}
