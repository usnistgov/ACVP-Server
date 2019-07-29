using NIST.CVP.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.Generation.KeyWrap.v1_0.TDES;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KeyWrap.Tests.TDES
{
    public class ParameterBuilder
    {
        private string _algorithm;

        private string _mode;
        private string[] _direction;
        private string[] _kwCipher;
        private MathDomain _ptLen;

        /// <summary>
        /// Provides a valid (as of construction) set of parameters
        /// </summary>
        /// <param name="mechanism"></param>
        /// <param name="mode"></param>
        public ParameterBuilder(KeyWrapType keyWrapType)
        {
            SetType(keyWrapType);

            _direction = ParameterValidator.VALID_DIRECTIONS;
            _kwCipher = ParameterValidator.VALID_KWCIPHER;
            
            SetMathRanges(keyWrapType);
        }

        public ParameterBuilder WithDirection(string[] value)
        {
            _direction = value;
            return this;
        }

        public ParameterBuilder WithKwCipher(string[] value)
        {
            _kwCipher = value;
            return this;
        }

        public ParameterBuilder WithPtLens(MathDomain value)
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
                Direction = _direction,
                KwCipher = _kwCipher,
                PayloadLen = _ptLen
            };
        }
        
        private void SetType(KeyWrapType keyWrapType)
        {
            switch (keyWrapType)
            {
                case KeyWrapType.TDES_KW:
                    _algorithm = "ACVP-TDES-KW";
                    _mode = string.Empty;
                    break;
            }
        }

        private void SetMathRanges(KeyWrapType keyWrapType)
        {
            switch (keyWrapType)
            {
                case KeyWrapType.TDES_KW:
                    _ptLen = new MathDomain()
                        .AddSegment(
                            new RangeDomainSegment(
                                new Random800_90(),
                                128,
                                ParameterValidator.MAXIMUM_PT_LEN,
                                ParameterValidator.PT_MODULUS
                            )
                        );
                    break;
            }
        }
    }
}
