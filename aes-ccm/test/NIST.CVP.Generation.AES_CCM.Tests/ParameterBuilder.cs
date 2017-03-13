using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CCM.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private int[] _keyLen;
        private Range _ptLen;
        private int[] _nonceLen;
        private Range _aadLen;
        private int[] _tagLen;

        public ParameterBuilder()
        {
            // Provides a valid (as of construction) set of parameters
            _algorithm = "AES-CCM";
            _keyLen = ParameterValidator.VALID_KEY_SIZES;
            _ptLen = new Range()
            {
                Min = ParameterValidator.VALID_MIN_PT,
                Max = ParameterValidator.VALID_MAX_PT
            };
            _nonceLen = ParameterValidator.VALID_NONCE_LENGTHS;
            _aadLen = new Range()
            {
                Min = ParameterValidator.VALID_MIN_AAD,
                Max = ParameterValidator.VALID_MAX_AAD
            };
            _tagLen = ParameterValidator.VALID_TAG_LENGTHS;
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

        public ParameterBuilder WithPtLen(Range value)
        {
            _ptLen = value;
            return this;
        }

        public ParameterBuilder WithNonceLen(int[] value)
        {
            _nonceLen = value;
            return this;
        }

        public ParameterBuilder WithAadLen(Range value)
        {
            _aadLen = value;
            return this;
        }

        public ParameterBuilder WithTagLen(int[] value)
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
                Nonce = _nonceLen,
                PtLen = _ptLen,
                AadLen = _aadLen,
                TagLen = _tagLen
            };
        }
    }
}
