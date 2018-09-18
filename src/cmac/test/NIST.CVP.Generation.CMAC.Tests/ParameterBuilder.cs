using System.Collections.Generic;

namespace NIST.CVP.Generation.CMAC.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private Capability[] _capabilities;
        

        public ParameterBuilder()
        {
            var capabilityBuilder = new CapabilityBuilder();

            _algorithm = "CMAC";
            _mode = "AES";
            _capabilities = new List<Capability>()
            {
                capabilityBuilder.Build()
            }.ToArray();
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
            Parameters p = new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                Capabilities = _capabilities,

                IsSample = false
            };

            return p;
        }
    }
}
