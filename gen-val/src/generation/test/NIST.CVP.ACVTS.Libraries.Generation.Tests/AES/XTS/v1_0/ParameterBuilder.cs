using NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XTS.v1_0
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private string[] _direction;
        private string[] _tweakModes;
        private int[] _keyLen;
        private MathDomain _ptLen;

        public ParameterBuilder()
        {
            // Provides a valid (as of construction) set of parameters
            _algorithm = "ACVP-AES";
            _mode = "XTS";
            _direction = ParameterValidator.VALID_DIRECTIONS;
            _tweakModes = ParameterValidator.VALID_TWEAKS;
            _keyLen = ParameterValidator.VALID_KEY_SIZES;
            _ptLen = new MathDomain();
            _ptLen.AddSegment(new RangeDomainSegment(new Random800_90(), ParameterValidator.MINIMUM_PT_LEN, ParameterValidator.MAXIMUM_PT_LEN));
        }

        public ParameterBuilder WithKeyLen(int[] value)
        {
            _keyLen = value;
            return this;
        }

        public ParameterBuilder WithDirection(string[] value)
        {
            _direction = value;
            return this;
        }

        public ParameterBuilder WithTweakMode(string[] value)
        {
            _tweakModes = value;
            return this;
        }

        public ParameterBuilder WithPtLen(MathDomain value)
        {
            _ptLen = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters()
            {
                Algorithm = _algorithm,
                Mode = _mode,
                KeyLen = _keyLen,
                Direction = _direction,
                TweakMode = _tweakModes,
                PayloadLen = _ptLen
            };
        }
    }
}
