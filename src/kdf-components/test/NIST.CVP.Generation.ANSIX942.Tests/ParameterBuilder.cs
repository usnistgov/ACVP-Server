using System;
using NIST.CVP.Math.Domain;
using NIST.CVP.Generation.KDF_Components.v1_0.ANSIX942;

namespace NIST.CVP.Generation.ANSIX942.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private string[] _kdfMode;
        private string[] _hashAlg;
        private MathDomain _otherInfoLen;
        private MathDomain _keyLen;
        private MathDomain _zzLen;

        public ParameterBuilder()
        {
            _algorithm = "kdf-components";
            _mode = "ansix9.42";
            _hashAlg = new[] { "sha2-224", "sha2-512" };
            _kdfMode = new[] { "DER", "concatenation" };
            _otherInfoLen = new MathDomain().AddSegment(new ValueDomainSegment(256));
            _zzLen = new MathDomain().AddSegment(new ValueDomainSegment(256));
            _keyLen = new MathDomain().AddSegment(new ValueDomainSegment(256));
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
            return this;
        }

        public ParameterBuilder WithMode(string value)
        {
            _mode = value;
            return this;
        }

        public ParameterBuilder WithKdfMode(string[] value)
        {
            _kdfMode = value;
            return this;
        }

        public ParameterBuilder WithHashAlg(string[] value)
        {
            _hashAlg = value;
            return this;
        }

        public ParameterBuilder WithOtherInfoLength(MathDomain value)
        {
            _otherInfoLen = value;
            return this;
        }

        public ParameterBuilder WithKeyLength(MathDomain value)
        {
            _keyLen = value;
            return this;
        }

        public ParameterBuilder WithZzLen(MathDomain value)
        {
            _zzLen = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                HashAlg = _hashAlg,
                KdfType = _kdfMode,
                OtherInfoLen = _otherInfoLen,
                ZzLen = _zzLen,
                KeyLen = _keyLen
            };
        }
    }
}
