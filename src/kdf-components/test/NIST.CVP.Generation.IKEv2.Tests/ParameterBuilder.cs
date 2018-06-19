using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.IKEv2.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private string[] _hashAlgs;
        private MathDomain _initNonceLengths;
        private MathDomain _respNonceLengths;
        private MathDomain _dhLengths;
        private MathDomain _dkmLengths;

        public ParameterBuilder()
        {
            _algorithm = "kdf-components";
            _mode = "ikev2";
            _hashAlgs = new [] {"sha-1", "sha2-384"};
            _initNonceLengths = new MathDomain().AddSegment(new ValueDomainSegment(128));
            _respNonceLengths = new MathDomain().AddSegment(new ValueDomainSegment(128));
            _dhLengths = new MathDomain().AddSegment(new ValueDomainSegment(256));
            _dkmLengths = new MathDomain().AddSegment(new ValueDomainSegment(384));
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

        public ParameterBuilder WithHashAlg(string[] value)
        {
            _hashAlgs = value;
            return this;
        }

        public ParameterBuilder WithInitNonceLengths(MathDomain value)
        {
            _initNonceLengths = value;
            return this;
        }

        public ParameterBuilder WithRespNonceLengths(MathDomain value)
        {
            _respNonceLengths = value;
            return this;
        }

        public ParameterBuilder WithDHLengths(MathDomain value)
        {
            _dhLengths = value;
            return this;
        }

        public ParameterBuilder WithDKMLengths(MathDomain value)
        {
            _dkmLengths = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,

                Capabilities = new []
                {
                    new Capabilities
                    {
                        HashAlg = _hashAlgs,
                        InitiatorNonceLength = _initNonceLengths,
                        ResponderNonceLength = _respNonceLengths,
                        DiffieHellmanSharedSecretLength = _dhLengths,
                        DerivedKeyingMaterialLength = _dkmLengths
                    }
                }
            };
        }
    }
}
