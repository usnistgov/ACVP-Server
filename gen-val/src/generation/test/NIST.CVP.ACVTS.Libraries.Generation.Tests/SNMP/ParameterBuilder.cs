using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SNMP;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SNMP
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private string[] _engineId;
        private MathDomain _passwordLength;

        public ParameterBuilder()
        {
            _algorithm = "kdf-components";
            _mode = "snmp";
            _engineId = new[] { "abcdef0123456789abcdef0123456789" };
            _passwordLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 64, 512, 8));
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

        public ParameterBuilder WithEngineId(string[] value)
        {
            _engineId = value;
            return this;
        }

        public ParameterBuilder WithPasswordLength(MathDomain value)
        {
            _passwordLength = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                EngineId = _engineId,
                PasswordLength = _passwordLength
            };
        }
    }
}
