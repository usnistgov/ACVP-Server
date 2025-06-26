using NIST.CVP.ACVTS.Libraries.Generation.HMAC.v2_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.HMAC.v2_0
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private MathDomain _keyLen;
        private MathDomain _macLen;
        private MathDomain _messageLen;

        /// <summary>
        /// Provides default parameters that are valid (as of construction)
        /// </summary>
        public ParameterBuilder()
        {
            _algorithm = "HMAC-SHA-1";
            _keyLen = new MathDomain().AddSegment(new ValueDomainSegment(8));
            _macLen = new MathDomain().AddSegment(new ValueDomainSegment(80));
            _messageLen = new MathDomain().AddSegment(new ValueDomainSegment(128));
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
            return this;
        }

        public ParameterBuilder WithKeyLen(MathDomain value)
        {
            _keyLen = value;
            return this;
        }

        public ParameterBuilder WithMacLen(MathDomain value)
        {
            _macLen = value;
            return this;
        }

        public ParameterBuilder WithMessageLen(MathDomain value)
        {
            _messageLen = value;
            return this;
        }
        
        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                KeyLen = _keyLen,
                MacLen = _macLen,
                MessageLen = _messageLen
            };
        }
    }
}
