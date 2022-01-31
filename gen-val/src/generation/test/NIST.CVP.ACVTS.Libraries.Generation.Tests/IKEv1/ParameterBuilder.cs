using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv1;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.IKEv1
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private Capability[] _capabilities;

        public ParameterBuilder()
        {
            _algorithm = "kdf-components";
            _mode = "ikev1";
            _capabilities = new[] { new CapabilityBuilder().Build() };
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

        public ParameterBuilder WithCapabilities(Capability[] value)
        {
            _capabilities = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                Capabilities = _capabilities
            };
        }
    }

    public class CapabilityBuilder
    {
        private string _authMode;
        private string[] _hashAlgs;
        private MathDomain _initNonceLengths;
        private MathDomain _respNonceLengths;
        private MathDomain _dhLengths;
        private MathDomain _pskLengths;

        public CapabilityBuilder()
        {
            _authMode = "dsa";
            _hashAlgs = new[] { "sha-1", "sha2-384" };
            _initNonceLengths = new MathDomain().AddSegment(new ValueDomainSegment(128));
            _respNonceLengths = new MathDomain().AddSegment(new ValueDomainSegment(128));
            _dhLengths = new MathDomain().AddSegment(new ValueDomainSegment(128));
            _pskLengths = new MathDomain().AddSegment(new ValueDomainSegment(128));
        }

        public CapabilityBuilder WithAuthenticationMode(string value)
        {
            _authMode = value;
            return this;
        }

        public CapabilityBuilder WithHashAlg(string[] value)
        {
            _hashAlgs = value;
            return this;
        }

        public CapabilityBuilder WithInitNonceLengths(MathDomain value)
        {
            _initNonceLengths = value;
            return this;
        }

        public CapabilityBuilder WithRespNonceLengths(MathDomain value)
        {
            _respNonceLengths = value;
            return this;
        }

        public CapabilityBuilder WithDHLengths(MathDomain value)
        {
            _dhLengths = value;
            return this;
        }

        public CapabilityBuilder WithPSKLengths(MathDomain value)
        {
            _pskLengths = value;
            return this;
        }

        public Capability Build()
        {
            return new Capability
            {
                AuthenticationMethod = _authMode,
                HashAlg = _hashAlgs,
                InitiatorNonceLength = _initNonceLengths,
                ResponderNonceLength = _respNonceLengths,
                DiffieHellmanSharedSecretLength = _dhLengths,
                PreSharedKeyLength = _pskLengths
            };
        }
    }
}
