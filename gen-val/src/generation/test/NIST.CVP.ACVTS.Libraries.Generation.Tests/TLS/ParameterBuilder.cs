using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v1_0;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TLS
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private TlsModes[] _versions;
        private string[] _hashAlgs;

        public ParameterBuilder()
        {
            _algorithm = "kdf-components";
            _mode = "tls";
            _versions = new[] { TlsModes.v10v11, TlsModes.v12 };
            _hashAlgs = new[] { "SHA2-384" };
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

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                TlsVersion = _versions,
                HashAlg = _hashAlgs
            };
        }
    }
}
