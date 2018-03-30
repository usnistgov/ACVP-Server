using System;
using System.Collections.Generic;
using System.Text;
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
            _capabilities = new [] {new CapabilityBuilder().Build()};
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
        private string _kdfMode;
        private string[] _macMode;
        private MathDomain _supportedLengths;
        private string[] _fixedDataOrder;
        private int[] _counterLength;
        private bool _supportsEmptyIv;

        public CapabilityBuilder()
        {
            _kdfMode = "counter";
            _macMode = new [] {"cmac-aes256", "hmac-sha224"};
            _supportedLengths = new MathDomain().AddSegment(new ValueDomainSegment(128));
            _fixedDataOrder = new[] {"middle fixed data"};
            _counterLength = new[] {8, 24};
            _supportsEmptyIv = false;
        }

        public CapabilityBuilder WithKdfMode(string value)
        {
            _kdfMode = value;
            return this;
        }

        public CapabilityBuilder WithMacMode(string[] value)
        {
            _macMode = value;
            return this;
        }

        public CapabilityBuilder WithSupportedLengths(MathDomain value)
        {
            _supportedLengths = value;
            return this;
        }

        public CapabilityBuilder WithFixedDataOrder(string[] value)
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

        public Capability Build()
        {
            return new Capability
            {
                KdfMode = _kdfMode,
                MacMode = _macMode,
                SupportedLengths = _supportedLengths,
                FixedDataOrder = _fixedDataOrder,
                CounterLength = _counterLength,
                SupportsEmptyIv = _supportsEmptyIv
            };
        }
    }
}
