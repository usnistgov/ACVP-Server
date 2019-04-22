using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.KDF_Components.v1_0.TLS;

namespace NIST.CVP.Generation.TLS.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private string[] _versions;
        private string[] _hashAlgs;

        public ParameterBuilder()
        {
            _algorithm = "kdf-components";
            _mode = "tls";
            _versions = new[] {"v1.0/1.1", "v1.2"};
            _hashAlgs = new[] {"sha2-384"};
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

        public ParameterBuilder WithVersion(string[] value)
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
