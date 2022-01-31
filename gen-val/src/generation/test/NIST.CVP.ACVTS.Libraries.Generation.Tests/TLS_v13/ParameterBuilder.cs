using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.TLSv13.RFC8446;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TLS_v13
{
    internal class ParameterBuilder
    {
        private string _algorithm = "TLS-v1.3";
        private string _mode = "KDF";
        private string _revision = "RFC8446";
        private IEnumerable<HashFunctions> _hashFunctions = new[] { HashFunctions.Sha2_d256 };
        private IEnumerable<TlsModes1_3> _runningModes = new[] { TlsModes1_3.DHE };

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

        public ParameterBuilder WithRevision(string value)
        {
            _revision = value;
            return this;
        }

        public ParameterBuilder WithHashAlgs(IEnumerable<HashFunctions> value)
        {
            _hashFunctions = value;
            return this;
        }

        public ParameterBuilder WithRunningMode(IEnumerable<TlsModes1_3> value)
        {
            _runningModes = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters()
            {
                Algorithm = _algorithm,
                Mode = _mode,
                Revision = _revision,
                HmacAlg = _hashFunctions.ToArray(),
                RunningMode = _runningModes.ToArray()
            };
        }
    }
}
