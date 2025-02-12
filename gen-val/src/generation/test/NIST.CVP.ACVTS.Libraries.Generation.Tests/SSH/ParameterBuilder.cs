﻿using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SSH;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SSH
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private string[] _ciphers;
        private string[] _hashAlgs;

        public ParameterBuilder()
        {
            _algorithm = "kdf-components";
            _mode = "ssh";
            _ciphers = new[] { "AES-192", "TDES" };
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

        public ParameterBuilder WithCipher(string[] value)
        {
            _ciphers = value;
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
                Cipher = _ciphers,
                HashAlg = _hashAlgs
            };
        }
    }
}
