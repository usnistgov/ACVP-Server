using NIST.CVP.ACVTS.Libraries.Generation.AES_CBC_CTS.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.CBC_CTS
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string[] _mode;
        private int[] _keyLen;
        private MathDomain _messageLen;

        public ParameterBuilder()
        {
            // Provides a valid (as of construction) set of parameters
            _algorithm = "ACVP-AES-ECB";
            _mode = ParameterValidator.ValidDirections;
            _keyLen = ParameterValidator.ValidKeySizes;
            _messageLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 65536, 1));
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

        public ParameterBuilder WithMessageLen(MathDomain value)
        {
            _messageLen = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters()
            {
                Algorithm = _algorithm,
                Revision = "1.0",
                KeyLen = _keyLen,
                Direction = _mode,
                PayloadLen = _messageLen
            };
        }
    }
}
