using NIST.CVP.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.Generation.KeyWrap.AES;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KeyWrap.Tests.AES
{
    public class ParameterBuilder
    {

        private KeyWrapType _keyWrapType;

        private string _algorithm;
        private string _mode;
        private int[] _keyLen;
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

            if (keyWrapType == KeyWrapType.AES_KW)
            {
                _keyLen = ParameterValidator.VALID_KEY_SIZES;
                _direction = ParameterValidator.VALID_DIRECTIONS;
                _kwCipher = ParameterValidator.VALID_KWCIPHER;
            }
            else if (keyWrapType == KeyWrapType.AES_KWP)
            {
                _keyLen = KeyWrapWithPadding.AES.ParameterValidator.VALID_KEY_SIZES;
                _direction = KeyWrapWithPadding.AES.ParameterValidator.VALID_DIRECTIONS;
                _kwCipher = KeyWrapWithPadding.AES.ParameterValidator.VALID_KWCIPHER;
            }


            SetMathRanges(keyWrapType);
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
                KeyLen = _keyLen,
                PtLen = _ptLen
            };
        }
        
        private void SetType(KeyWrapType keyWrapType)
        {
            _keyWrapType = keyWrapType;

            switch (keyWrapType)
            {
                case KeyWrapType.AES_KW:
                    _algorithm = "AES";
                    _mode = "KW";
                    break;

                case KeyWrapType.AES_KWP:
                    _algorithm = "AES";
                    _mode = "KWP";
                    break;
            }
        }

        private void SetMathRanges(KeyWrapType keyWrapType)
        {
            switch (keyWrapType)
            {
                case KeyWrapType.AES_KW:
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

                case KeyWrapType.AES_KWP:
                    _ptLen = new MathDomain()
                        .AddSegment(
                            new RangeDomainSegment(
                                new Random800_90(), 
                                KeyWrapWithPadding.AES.ParameterValidator.MINIMUM_PT_LEN,
                                KeyWrapWithPadding.AES.ParameterValidator.MAXIMUM_PT_LEN,
                                KeyWrapWithPadding.AES.ParameterValidator.PT_MODULUS
                            )
                        );
                    break;
            }
        }
    }
}
