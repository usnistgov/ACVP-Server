using NIST.CVP.Generation.AES_CFB8.v1_0;

namespace NIST.CVP.Generation.AES_CFB8.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string[] _mode;
        private int[] _keyLen;

        public ParameterBuilder()
        {
            // Provides a valid (as of construction) set of parameters
            _algorithm = "AES-ECB";
            _mode = ParameterValidator.VALID_DIRECTIONS;
            _keyLen = ParameterValidator.VALID_KEY_SIZES;
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
            return this;
        }

        public ParameterBuilder WithMode(string[] value)
        {
            _mode = value;
            return this;
        }

        public ParameterBuilder WithKeyLen(int[] value)
        {
            _keyLen = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters()
            {
                Algorithm = _algorithm,
                Revision = "1.0",
                KeyLen = _keyLen,
                Direction = _mode
            };
        }
    }
}
