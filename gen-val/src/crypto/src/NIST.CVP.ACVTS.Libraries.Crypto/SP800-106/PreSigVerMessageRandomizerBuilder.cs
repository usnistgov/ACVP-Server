using NIST.CVP.ACVTS.Libraries.Crypto.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.SP800_106;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto
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
