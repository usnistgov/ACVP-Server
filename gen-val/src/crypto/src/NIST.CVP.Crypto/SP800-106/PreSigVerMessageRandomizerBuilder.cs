using NIST.CVP.Crypto.Common;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto
{
    public class PreSigVerMessageRandomizerBuilder : IPreSigVerMessageRandomizerBuilder
    {
        private IEntropyProvider _entropyProvider;

        public PreSigVerMessageRandomizerBuilder(IEntropyProviderFactory entropyProviderFactory)
        {
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public IPreSigVerMessageRandomizerBuilder WithEntropyProvider(IEntropyProvider entropyProvider)
        {
            _entropyProvider = entropyProvider;
            return this;
        }

        public IPreSigVerMessageRandomizer Build()
        {
            return new PreSigVerMessageRandomizer(_entropyProvider);
        }
    }
}