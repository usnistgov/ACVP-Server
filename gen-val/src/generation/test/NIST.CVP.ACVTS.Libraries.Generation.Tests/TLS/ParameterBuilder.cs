using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TLS
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private string _revision;
        private TlsModes[] _versions;
        private string[] _hashAlgs;
        private MathDomain _keyBlockLength;

        public ParameterBuilder()
        {
            _algorithm = "kdf-components";
            _mode = "tls";
            _revision = "1.0";
            _versions = new[] { TlsModes.v10v11, TlsModes.v12 };
            _hashAlgs = new[] { "SHA2-384" };
            _keyBlockLength = new MathDomain().AddSegment(new ValueDomainSegment(1024));
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

        public ParameterBuilder WithRevision(string value)
        {
            _revision = value;
            return this;
        }

        public ParameterBuilder WithVersion(TlsModes[] value)
        {
            _versions = value;
            return this;
        }

        public ParameterBuilder WithHashAlg(string[] value)
        {
            _hashAlgs = value;
            return this;
        }

        public ParameterBuilder WithKeyBlockLength(MathDomain value)
        {
            _keyBlockLength = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                Revision = _revision,
                TlsVersion = _versions,
                HashAlg = _hashAlgs,
                KeyBlockLength = _keyBlockLength
            };
        }
    }
}
