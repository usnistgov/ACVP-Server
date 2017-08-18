using NIST.CVP.Crypto.KeyWrap.Enums;
using NIST.CVP.Generation.KeyWrap.TDES;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KeyWrap.Tests.TDES
{
    public class ParameterBuilder
    {

        private KeyWrapType _keyWrapType;

        private string _algorithm;
        //private int[] _keyLen;
        private string[] _direction;
        private string[] _kwCipher;
        private MathDomain _ptLen;
        private int[] _keyingOption;


        /// <summary>
        /// Provides a valid (as of construction) set of parameters
        /// </summary>
        /// <param name="mechanism"></param>
        /// <param name="mode"></param>
        public ParameterBuilder(KeyWrapType keyWrapType)
        {
            SetType(keyWrapType);

            //_keyLen = ParameterValidator.VALID_KEY_SIZES;
            _direction = ParameterValidator.VALID_DIRECTIONS;
            _kwCipher = ParameterValidator.VALID_KWCIPHER;
            _keyingOption = ParameterValidator.VALID_KEYING_OPTIONS;

            SetMathRanges(keyWrapType);
        }

        public ParameterBuilder WithAlgorithm(KeyWrapType value)
        {
            SetType(value);
            return this;
        }

        //public ParameterBuilder WithKeyLen(int[] value)
        //{
        //    _keyLen = value;
        //    return this;
        //}

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
                KeyingOption = _keyingOption,
                Direction = _direction,
                KwCipher = _kwCipher,
                //KeyLen = _keyLen,
                PtLen = _ptLen
            };
        }
        
        private void SetType(KeyWrapType keyWrapType)
        {
            _keyWrapType = keyWrapType;

            switch (keyWrapType)
            {
                case KeyWrapType.TDES_KW:
                    _algorithm = "TDES-KW";
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
