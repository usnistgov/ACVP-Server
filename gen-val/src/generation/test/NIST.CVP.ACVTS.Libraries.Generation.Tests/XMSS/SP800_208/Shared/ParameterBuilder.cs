using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.Shared;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.XMSS.SP800_208.Shared
{
    /// <summary>
    /// Fluent builder for <see cref="Parameters"/> used in XMSS SP800-208 tests.
    /// Mirrors the builder pattern used for other algorithms.
    /// </summary>
    public class ParameterBuilder
    {
        private string _algorithm = "XMSS";
        private string _mode = "sigGen"; // default to sigGen
        private string _revision = "SP800-208";
        private MathDomain _messageLength = new MathDomain().AddSegment(new ValueDomainSegment(256));
        private GeneralCapabilities _generalCapabilities = new()
        {
            XmssModes = [XmssMode.XMSS_SHA2_10_256]
        };

        public ParameterBuilder WithAlgoModeRevision(string algorithm, string mode, string revision)
        {
            _algorithm = algorithm;
            _mode = mode;
            _revision = revision;
            return this;
        }

        public ParameterBuilder WithMessageLength(MathDomain value)
        {
            _messageLength = value;
            return this;
        }

        public ParameterBuilder WithGeneralCapabilities(GeneralCapabilities value)
        {
            _generalCapabilities = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                Revision = _revision,
                MessageLength = _messageLength,
                Capabilities = _generalCapabilities
            };
        }
    }
}
