using System.Linq;
using NIST.CVP.Generation.AES_CCM.v1_0;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_CCM.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private int[] _keyLen;
        private MathDomain _ptLen;
        private MathDomain _nonceLen;
        private MathDomain _aadLen;
        private MathDomain _tagLen;

        public ParameterBuilder()
        {
            // Provides a valid (as of construction) set of parameters
            _algorithm = "ACVP-AES-CCM";
            _keyLen = ParameterValidator.VALID_KEY_SIZES;

            Random800_90 random = new Random800_90();

            _ptLen = new MathDomain();
            _ptLen.AddSegment(new RangeDomainSegment(random, 0, 32 * 8, 8));

            _aadLen = new MathDomain();
            _aadLen.AddSegment(new RangeDomainSegment(random, 0, (1 << 19), 8));

            _tagLen = new MathDomain();
            ParameterValidator.VALID_TAG_LENGTHS
                .ToList()
                .ForEach(fe => _tagLen.AddSegment(new ValueDomainSegment(fe)));
            
            _nonceLen = new MathDomain();
            ParameterValidator.VALID_NONCE_LENGTHS
                .ToList()
                .ForEach(fe => _nonceLen.AddSegment(new ValueDomainSegment(fe)));
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
            return this;
        }

        public ParameterBuilder WithKeyLen(int[] value)
        {
            _keyLen = value;
            return this;
        }

        public ParameterBuilder WithPtLen(MathDomain value)
        {
            _ptLen = value;
            return this;
        }

        public ParameterBuilder WithNonceLen(MathDomain value)
        {
            _nonceLen = value;
            return this;
        }

        public ParameterBuilder WithAadLen(MathDomain value)
        {
            _aadLen = value;
            return this;
        }

        public ParameterBuilder WithTagLen(MathDomain value)
        {
            _tagLen = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters()
            {
                Algorithm = _algorithm,

                KeyLen = _keyLen,
                IvLen = _nonceLen,
                PayloadLen = _ptLen,
                AadLen = _aadLen,
                TagLen = _tagLen
            };
        }
    }
}
