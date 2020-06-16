using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.PBKDF.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string[] _hashAlgs;
        private MathDomain _saltLen;
        private MathDomain _passLen;
        private MathDomain _keyLen;
        private MathDomain _itrCount;

        public ParameterBuilder()
        {
            _algorithm = "PBKDF";
            _hashAlgs = new [] {"sha-1", "sha2-384"};
            _saltLen = new MathDomain().AddSegment(new ValueDomainSegment(128));
            _passLen = new MathDomain().AddSegment(new ValueDomainSegment(32));
            _keyLen = new MathDomain().AddSegment(new ValueDomainSegment(256));
            _itrCount = new MathDomain().AddSegment(new ValueDomainSegment(100));
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
            return this;
        }

        public ParameterBuilder WithHashAlg(string[] value)
        {
            _hashAlgs = value;
            return this;
        }

        public ParameterBuilder WithKeyLength(MathDomain value)
        {
            _keyLen = value;
            return this;
        }

        public ParameterBuilder WithPasswordLength(MathDomain value)
        {
            _passLen = value;
            return this;
        }

        public ParameterBuilder WithSaltLength(MathDomain value)
        {
            _saltLen = value;
            return this;
        }

        public ParameterBuilder WithIterationCount(MathDomain value)
        {
            _itrCount = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Capabilities = new []
                {
                    new Capability
                    {
                        HashAlg = _hashAlgs,
                        KeyLength = _keyLen,
                        PasswordLength = _passLen,
                        SaltLength = _saltLen,
                        IterationCount = _itrCount
                    }
                }
            };
        }
    }
}