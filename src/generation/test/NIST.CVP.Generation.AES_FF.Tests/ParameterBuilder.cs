using NIST.CVP.Generation.AES_FFX.v1_0.Base;
using NIST.CVP.Math.Domain;
using System.Collections.Generic;

namespace NIST.CVP.Generation.AES_FF.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string[] _direction;
        private int[] _keyLen;
        private MathDomain _tweakLen;
        private Capability[] _capabilities;

        public ParameterBuilder()
        {
            // Provides a valid (as of construction) set of parameters
            _algorithm = "ACVP-AES-FF3-1";
            _direction = ParameterValidator.VALID_DIRECTIONS;
            _keyLen = ParameterValidator.VALID_KEY_SIZES;
            _tweakLen = new MathDomain().AddSegment(new ValueDomainSegment(56));
            _capabilities = new List<Capability>()
            {
                new Capability()
                {
                    Radix = 10,
                    Alphabet = "0123456789",
                    MinLen = 10,
                    MaxLen = 26
                }
            }.ToArray();
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
            return this;
        }

        public ParameterBuilder WithDirection(string[] value)
        {
            _direction = value;
            return this;
        }

        public ParameterBuilder WithKeyLen(int[] value)
        {
            _keyLen = value;
            return this;
        }

        public ParameterBuilder WithTweakLen(MathDomain value)
        {
            _tweakLen = value;
            return this;
        }

        public ParameterBuilder WithCapabilities(Capability[] value)
        {
            _capabilities = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters()
            {
                Algorithm = _algorithm,
                Revision = "1.0",
                KeyLen = _keyLen,
                Direction = _direction,
                TweakLen = _tweakLen,
                Capabilities = _capabilities
            };
        }
    }
}
