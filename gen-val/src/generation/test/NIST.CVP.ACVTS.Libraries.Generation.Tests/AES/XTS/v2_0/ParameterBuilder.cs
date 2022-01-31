using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XTS.v2_0
{
    public class ParameterBuilder
    {
        private BlockCipherDirections[] _direction;
        private XtsTweakModes[] _tweakModes;
        private int[] _keyLen;
        private MathDomain _ptLen;
        private bool _ptLenMatch;
        private MathDomain _dataUntLen;

        public ParameterBuilder()
        {
            // Provides a valid (as of construction) set of parameters
            _direction = new[] { BlockCipherDirections.Encrypt };
            _tweakModes = new[] { XtsTweakModes.Hex };
            _keyLen = ParameterValidator.VALID_KEY_SIZES;
            _ptLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), ParameterValidator.MINIMUM_PT_LEN, ParameterValidator.MAXIMUM_PT_LEN));
            _dataUntLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), ParameterValidator.MINIMUM_PT_LEN, ParameterValidator.MAXIMUM_PT_LEN));
            _ptLenMatch = false;
        }

        public ParameterBuilder WithKeyLen(int[] value)
        {
            _keyLen = value;
            return this;
        }

        public ParameterBuilder WithDirection(BlockCipherDirections[] value)
        {
            _direction = value;
            return this;
        }

        public ParameterBuilder WithTweakMode(XtsTweakModes[] value)
        {
            _tweakModes = value;
            return this;
        }

        public ParameterBuilder WithPtLen(MathDomain value)
        {
            _ptLen = value;
            return this;
        }

        public ParameterBuilder WithDataUnitLen(MathDomain value)
        {
            _dataUntLen = value;
            return this;
        }

        public ParameterBuilder WithMatchPtLen(bool value)
        {
            _ptLenMatch = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters()
            {
                Algorithm = "AES",
                Mode = "XTS",
                Revision = "2.0",
                KeyLen = _keyLen,
                Direction = _direction,
                TweakMode = _tweakModes,
                PayloadLen = _ptLen,
                DataUnitLenMatchesPayload = _ptLenMatch,
                DataUnitLen = _dataUntLen
            };
        }
    }
}
