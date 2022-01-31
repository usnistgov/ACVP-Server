using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KES
{
    public class RsaSveBuilder : IRsaSveBuilder
    {
        private IRsa _rsa;
        private IEntropyProvider _entropProvider;

        public IRsaSveBuilder WithRsa(IRsa value)
        {
            _rsa = value;
            return this;
        }

        public IRsaSveBuilder WithEntropyProvider(IEntropyProvider value)
        {
            _entropProvider = value;
            return this;
        }

        public IRsaSve Build()
        {
            return new RsaSve(_rsa, _entropProvider);
        }
    }
}
