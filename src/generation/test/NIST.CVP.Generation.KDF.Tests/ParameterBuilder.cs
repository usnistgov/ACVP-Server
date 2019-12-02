using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.KDF.v1_0;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KDF.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private Capability[] _capabilities;

        public ParameterBuilder()
        {
            _algorithm = "KDF";
            _mode = "";
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
        private KdfModes _kdfMode;
        private MacModes[] _macMode;
        private MathDomain _supportedLengths;
        private CounterLocations[] _fixedDataOrder;
        private int[] _counterLength;
        private bool _supportsEmptyIv;
        private bool _requiresEmptyIv;

        public CapabilityBuilder()
        {
            _kdfMode = KdfModes.Counter;
            _macMode = new[] { MacModes.CMAC_AES256, MacModes.HMAC_SHA224 };
            _supportedLengths = new MathDomain().AddSegment(new ValueDomainSegment(128));
            _fixedDataOrder = new[] { CounterLocations.MiddleFixedData };
            _counterLength = new[] { 8, 24 };
            _supportsEmptyIv = false;
            _requiresEmptyIv = false;
        }

        public CapabilityBuilder WithKdfMode(KdfModes value)
        {
            _kdfMode = value;
            return this;
        }

        public CapabilityBuilder WithMacMode(MacModes[] value)
        {
            _macMode = value;
            return this;
        }

        public CapabilityBuilder WithSupportedLengths(MathDomain value)
        {
            _supportedLengths = value;
            return this;
        }

        public CapabilityBuilder WithFixedDataOrder(CounterLocations[] value)
        {
            _fixedDataOrder = value;
            return this;
        }

        public CapabilityBuilder WithCounterLength(int[] value)
        {
            _counterLength = value;
            return this;
        }

        public CapabilityBuilder WithSupportsEmptyIv(bool value)
        {
            _supportsEmptyIv = value;
            return this;
        }

        public CapabilityBuilder WithRequiresEmptyIv(bool value)
        {
            _requiresEmptyIv = value;
            return this;
        }

        public Capability Build()
        {
            return new Capability
            {
                KdfMode = _kdfMode,
                MacMode = _macMode,
                SupportedLengths = _supportedLengths,
                FixedDataOrder = _fixedDataOrder,
                CounterLength = _counterLength,
                SupportsEmptyIv = _supportsEmptyIv,
                RequiresEmptyIv = _requiresEmptyIv
            };
        }
    }
}
