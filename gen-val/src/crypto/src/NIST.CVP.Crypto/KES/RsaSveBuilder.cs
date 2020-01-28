using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KES
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