using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX942;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ANSIX942
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private AnsiX942Types[] _kdfMode;
        private string[] _hashAlg;
        private AnsiX942Oids[] _oid;
        private MathDomain _otherInfoLen;
        private MathDomain _suppInfoLen;
        private MathDomain _keyLen;
        private MathDomain _zzLen;

        public ParameterBuilder()
        {
            _algorithm = "kdf-components";
            _mode = "ansix9.42";
            _hashAlg = new[] { "SHA2-224", "SHA2-512" };
            _kdfMode = EnumHelpers.GetEnumsWithoutDefault<AnsiX942Types>().ToArray();
            _oid = EnumHelpers.GetEnumsWithoutDefault<AnsiX942Oids>().ToArray();
            _otherInfoLen = new MathDomain().AddSegment(new ValueDomainSegment(256));
            _suppInfoLen = new MathDomain().AddSegment(new ValueDomainSegment(256));
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

        public ParameterBuilder WithKdfMode(AnsiX942Types[] value)
        {
            _kdfMode = value;
            return this;
        }

        public ParameterBuilder WithOid(AnsiX942Oids[] value)
        {
            _oid = value;
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

        public ParameterBuilder WithSuppInfoLength(MathDomain value)
        {
            _suppInfoLen = value;
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
                Oid = _oid,
                OtherInfoLen = _otherInfoLen,
                SuppInfoLen = _suppInfoLen,
                ZzLen = _zzLen,
                KeyLen = _keyLen
            };
        }
    }
}
