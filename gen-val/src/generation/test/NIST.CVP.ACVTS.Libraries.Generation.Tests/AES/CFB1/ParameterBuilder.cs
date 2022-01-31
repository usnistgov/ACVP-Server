using NIST.CVP.ACVTS.Libraries.Generation.AES_CFB1.v1_0;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.CFB1
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string[] _mode;
        private string _revision;
        private int[] _keyLen;

        public ParameterBuilder()
        {
            // Provides a valid (as of construction) set of parameters
            _algorithm = "ACVP-AES-CCM";
            _mode = ParameterValidator.VALID_DIRECTIONS;
            _revision = "1.0";
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

        public ParameterBuilder WithRevision(string value)
        {
            _revision = value;
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

                Revision = _revision,
                KeyLen = _keyLen,
                Direction = _mode
            };
        }
    }
}
