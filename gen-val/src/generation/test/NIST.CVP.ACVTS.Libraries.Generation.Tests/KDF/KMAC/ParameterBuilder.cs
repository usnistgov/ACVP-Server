using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDF.SP800_108r1.KMAC;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KDF.KMAC
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private string _revision;
        private MacModes[] _macMode;
        private MathDomain _keyDerivationKeyLength;
        private MathDomain _contextLength;
        private MathDomain _labelLength;
        private MathDomain _derivedKeyLength;

        public ParameterBuilder()
        {
            _algorithm = "KDF";
            _mode = "KMAC";
            _revision = "SP800-108r1";
            _macMode = new[] { MacModes.KMAC_128 };
            _keyDerivationKeyLength = new MathDomain().AddSegment(new ValueDomainSegment(1024));
            _contextLength = new MathDomain().AddSegment(new ValueDomainSegment(1024));
            _labelLength = new MathDomain().AddSegment(new ValueDomainSegment(1024));
            _derivedKeyLength = new MathDomain().AddSegment(new ValueDomainSegment(1024));
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

        public ParameterBuilder WithMacMode(MacModes[] value)
        {
            _macMode = value;
            return this;
        }
        
        public ParameterBuilder WithKeyDerivationKeyLength(MathDomain value)
        {
            _keyDerivationKeyLength = value;
            return this;
        }

        public ParameterBuilder WithContextLength(MathDomain value)
        {
            _contextLength = value;
            return this;
        }
        
        public ParameterBuilder WithLabelLength(MathDomain value)
        {
            _labelLength = value;
            return this;
        }
        
        public ParameterBuilder WithDerivedKeyLength(MathDomain value)
        {
            _derivedKeyLength = value;
            return this;
        }
        
        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                Revision = _revision,
                MacMode = _macMode,
                KeyDerivationKeyLength = _keyDerivationKeyLength,
                ContextLength = _contextLength,
                LabelLength = _labelLength,
                DerivedKeyLength = _derivedKeyLength
            };
        }
    }
}
