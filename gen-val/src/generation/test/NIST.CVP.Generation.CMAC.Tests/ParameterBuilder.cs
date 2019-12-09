using System.Collections.Generic;
using NIST.CVP.Generation.CMAC.v1_0;

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

            _algorithm = "CMAC-AES";
            _mode = string.Empty;
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
                Revision = "1.0",
                Capabilities = _capabilities,

                IsSample = false
            };

            return p;
        }
    }
}
